namespace ValhallaBebidas.Domain.Interfaces;

/// <summary>
/// Representa o padrão Unit of Work do EF Core.
/// Permite que a camada de Service controle quando o commit acontece,
/// tornando operações multi-entidade atômicas.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
