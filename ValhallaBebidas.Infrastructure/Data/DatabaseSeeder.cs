using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ValhallaBebidasDbContext db)
    {
        await SeedCategoriasAsync(db);
        await SeedAdminAsync(db);
    }

    private static async Task SeedCategoriasAsync(ValhallaBebidasDbContext db)
    {
        if (await db.Categorias.AnyAsync()) return;

        db.Categorias.AddRange(
            new Categoria { Nome = "Refrigerantes" },
            new Categoria { Nome = "Destilados" },
            new Categoria { Nome = "Cervejas" },
            new Categoria { Nome = "Energéticos" },
            new Categoria { Nome = "Águas" },
            new Categoria { Nome = "Sucos" },
            new Categoria { Nome = "Vinhos" },
            new Categoria { Nome = "Gelos" }
        );

        await db.SaveChangesAsync();
    }

    public static async Task SeedAdminAsync(ValhallaBebidasDbContext db)
    {
        if (await db.Funcionarios.AnyAsync()) return;

        var hash = BCrypt.Net.BCrypt.HashPassword("adminValhalla");

        var endereco = db.Enderecos.FirstOrDefault();
        if (endereco == null)
        {
            endereco = new Endereco
            {
                Logradouro = "Rua Vicente Garcia",
                Numero = 24,
                Complemento = "Fundos",
                Cep = "08440-261",
                Bairro = "Guaianases",
                Cidade = "São Paulo",
                Estado = "SP",
            };
            db.Enderecos.Add(endereco);
            await db.SaveChangesAsync();
        }

        db.Funcionarios.Add(new Funcionario
        {
            NomeCompleto = "Administrador",
            DataNascimento = new DateTime(2000, 1, 1),
            Cpf = "11122233344",
            Telefone = "(11) 99999-9999",
            Email = "admin@valhalla.br",
            SenhaHash = hash,
            Status = true,
            EnderecoId = endereco.Id,
        });

        await db.SaveChangesAsync();
    }
}
