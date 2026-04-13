using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ValhallaBebidas.Infrastructure.Data;

/// <summary>
/// Fábrica usada pelas ferramentas do EF Core (migrations) em tempo de design.
///
/// CONTEXTO DIDÁTICO:
/// O comando "dotnet ef migrations add" precisa instanciar o DbContext para
/// ler o modelo e gerar o SQL da migration. Porém, o DbContext desta aplicação
/// usa injeção de dependência (recebe DbContextOptions pelo construtor) — ou seja,
/// não tem um construtor sem parâmetros que o EF Core possa chamar diretamente.
///
/// A interface IDesignTimeDbContextFactory resolve isso: ela diz ao EF Core
/// "use este método para criar o DbContext quando estiver gerando migrations",
/// sem precisar do Program.cs ou do projeto de startup.
///
/// Isso permite rodar: dotnet ef migrations add <Nome> --project ValhallaBebidas.Infrastructure
/// de qualquer lugar, sem precisar especificar --startup-project.
/// </summary>
public class ValhallaBebidasDbContextFactory : IDesignTimeDbContextFactory<ValhallaBebidasDbContext>
{
    public ValhallaBebidasDbContextFactory()
    {
    }

    public ValhallaBebidasDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ValhallaBebidasDbContext>();

        // String de conexão usada APENAS para gerar as migrations.
        // Em produção, a string de conexão vem do appsettings.json configurado na API.
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=ValhallaBebidasDb;Trusted_Connection=True;");

        return new ValhallaBebidasDbContext(optionsBuilder.Options);
    }
}
