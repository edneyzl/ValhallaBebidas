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



    // ──────────────────────────────────────────────────────────────────────────────
    // CATEGORIAS
    // ──────────────────────────────────────────────────────────────────────────────
    private static async Task SeedCategoriasAsync(ValhallaBebidasDbContext db)
    {
        // Só insere se a tabela estiver vazia
        if (await db.Categorias.AnyAsync()) return;

        db.Categorias.AddRange(
            new Categoria { Nome = "Refrigerantes" },
            new Categoria { Nome = "Destilados"},
            new Categoria { Nome = "Cervejas" },
            new Categoria { Nome = "Energéticos"},
            new Categoria { Nome = "Águas"},
            new Categoria { Nome = "Sucos" },
            new Categoria { Nome = "Vinhos" },
            new Categoria { Nome = "Gelos" }
        );

        await db.SaveChangesAsync();
        Console.WriteLine("✅ Seed: 8 categorias inseridas.");
    }


    // ──────────────────────────────────────────────────────────────────────────────
    // ENDEREÇOS
    // ──────────────────────────────────────────────────────────────────────────────
    private static async Task SeedEnderecosAsync(ValhallaBebidasDbContext db)
    {
        // Só insere se a tabela estiver vazia
        if (await db.Enderecos.AnyAsync()) return;

        db.Enderecos.AddRange(
            new Endereco { TipoLogradouro = "Rua", Logradouro="Vicente Garcia", Numero=24, Complemento="Fundos", Cep="08440-261", 
                Bairro = "Guaianases", Cidade = "São Paulo", Estado = "SP" }
        );

        await db.SaveChangesAsync();
        Console.WriteLine("✅ Seed: 1 endereço inserido.");
    }


    // ──────────────────────────────────────────────────────────────────────────────
    // FUNCIONÁRIOS
    // ──────────────────────────────────────────────────────────────────────────────

    private static async Task SeedUsuariosAsync(ValhallaBebidasDbContext db)
    {
        // Só insere se a tabela estiver vazia
        if (await db.Funcionarios.AnyAsync()) return;

        // Senha "1234" para todos os usuários de teste
        // (GerarHash replica o algoritmo do UsuarioService)
        var hash = GerarHash("adminValhalla");

        db.Funcionarios.AddRange(
            new Funcionario { NomeCompleto = "Administrador", DataNascimento= new DateTime(01,01,2000), Cpf="111.222.333-44", 
                Telefone="(11) 99999-9999", Email = "admin@valhalla.br", Login = "admin", SenhaHash = hash, Status=true,EnderecoId=1 }
        );

        await db.SaveChangesAsync();
        Console.WriteLine("✅ Seed: 1 usuário inserido.");
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
