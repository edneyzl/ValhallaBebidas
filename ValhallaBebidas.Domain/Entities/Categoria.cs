namespace ValhallaBebidas.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set;} = string.Empty;

    /// <summary>
    /// Coleção de produtos pertencentes a esta categoria.
    /// Relacionamento: 1 Categoria → N Produtos.
    /// </summary>
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
