using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAdminAsync(ValhallaBebidasDbContext db)
    {
        if (await db.Funcionarios.AnyAsync()) return;

        var hash = BCrypt.Net.BCrypt.HashPassword("adminValhalla");

        var endereco = db.Enderecos.FirstOrDefault();
        if (endereco == null)
        {
            endereco = new Endereco
            {
                TipoLogradouro = "Rua",
                Logradouro = "Vicente Garcia",
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
            Login = "admin",
            SenhaHash = hash,
            Status = true,
            EnderecoId = endereco.Id,
        });

        await db.SaveChangesAsync();
    }
}
