using System;
using System.Threading.Tasks;
using TransaccionesEFCore.DATA;
using TransaccionesEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace TransaccionesEFCore
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("==== GESTIÓN DE USUARIOS ====\n");

            Console.WriteLine("1. Realizar transferencia");
            Console.WriteLine("2. Borrar usuario");
            Console.Write("\nSeleccione una opción (1 o 2): ");
            string seleccion = Console.ReadLine();

            if (seleccion == "1")
            {
                Console.Write("\nIngrese el ID del remitente: ");
                int origenId = int.Parse(Console.ReadLine());

                Console.Write("Ingrese el ID del receptor: ");
                int destinoId = int.Parse(Console.ReadLine());

                Console.Write("Monto a transferir: $");
                decimal cantidad = decimal.Parse(Console.ReadLine());

                await RealizarTransferencia(origenId, destinoId, cantidad);
            }
            else if (seleccion == "2")
            {
                Console.Write("\nIngrese el ID del usuario a eliminar: ");
                int usuarioId = int.Parse(Console.ReadLine());

                await BorrarUsuario(usuarioId);
            }
            else
            {
                Console.WriteLine("⚠️ Opción inválida.");
            }

            Console.WriteLine("\n[INFO] Pulsa cualquier tecla para cerrar...");
            Console.ReadKey();
        }

        static async Task RealizarTransferencia(int remitenteId, int receptorId, decimal valor)
        {
            using var contexto = new BaseContext();
            using var transaccion = await contexto.Database.BeginTransactionAsync();

            try
            {
                var remitente = await contexto.Usuarios.FindAsync(remitenteId);
                var receptor = await contexto.Usuarios.FindAsync(receptorId);

                if (remitente == null || receptor == null)
                    throw new Exception("Uno o ambos usuarios no existen.");

                if (remitente.Cuenta < valor)
                    throw new Exception("Fondos insuficientes para la operación.");

                remitente.Cuenta -= valor;
                receptor.Cuenta += valor;

                await contexto.SaveChangesAsync();
                await transaccion.CommitAsync();

                Console.WriteLine("✅ Transferencia completada exitosamente.");
            }
            catch (Exception error)
            {
                await transaccion.RollbackAsync();
                Console.WriteLine($"❌ Error durante la operación: {error.Message}");
            }
        }

        static async Task BorrarUsuario(int idUsuario)
        {
            using var contexto = new BaseContext();
            using var transaccion = await contexto.Database.BeginTransactionAsync();

            try
            {
                var usuario = await contexto.Usuarios.FindAsync(idUsuario);
                if (usuario == null)
                    throw new Exception("El usuario no fue encontrado.");

                if (usuario.Cuenta != 0)
                    throw new Exception("No se puede eliminar: el saldo debe ser igual a 0.");

                contexto.Usuarios.Remove(usuario);
                await contexto.SaveChangesAsync();
                await transaccion.CommitAsync();

                Console.WriteLine("✅ Usuario eliminado exitosamente.");
            }
            catch (Exception error)
            {
                await transaccion.RollbackAsync();
                Console.WriteLine($"❌ Error al eliminar: {error.Message}");
            }
        }
    }
}
