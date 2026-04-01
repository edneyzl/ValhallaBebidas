using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Infrastructure.Data;

public static class DatabaseSeeder
{

    public static async Task SeedAsync(ValhallaBebidasDbContext db)
    {
        // Aplica migrations pendentes e garante que o banco existe
        await db.Database.MigrateAsync();

        // A ordem importa: seed na ordem das dependências (FK)
        await SeedCategoriasAsync(db);
    }

    private static async Task SeedCategoriasAsync(ValhallaBebidasDbContext db)
    {
        // Só insere se a tabela estiver vazia
        if (await db.Categorias.AnyAsync()) return;

        // (GerarHash replica o algoritmo do UsuarioService)
        var hash1234 = GerarHash("valhalla123");

        db.Categorias.AddRange(
            new Categoria { Nome = "Refrigerantes" },
            new Categoria { Nome = "Destilados"},
            new Categoria { Nome = "Cervejas" },
            new Categoria { Nome = "Energéticos"},
            new Categoria { Nome = "Águas"}
        );

        await db.SaveChangesAsync();
        Console.WriteLine("✅ Seed: 5 categorias inseridas.");
    }




    // ──────────────────────────────────────────────────────────────────────────────
    // AUXILIAR: hash SHA256 igual ao UsuarioService
    // ──────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Replica o GerarHash do UsuarioService para que as senhas do seed
    /// sejam válidas no login. Não usar em produção — use BCrypt.
    /// </summary>
    private static string GerarHash(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLower();
    }
}
