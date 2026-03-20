using System.Numerics;
using System.ComponentModel.DataAnnotations;

namespace ValhallaBebidas.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string EanCodBarras { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; } 
    public decimal PrecoCusto { get; set; }
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Status { get; set; }
    /// <summary>
    /// Chave estrangeira para a categoria do produto.
    /// Opcional — produto pode existir sem categoria.
    /// </summary>
    public int CategoriaId { get; set; }

    /// <summary>
    /// Caminho relativo para a foto do produto.
    /// Opcional, salvo dentro de uploads/produtos.
    /// </summary>
    public string? FotoProduto { get; set; }

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 
    /// <summary>
    /// Referência à categoria (objeto completo).
    /// Relacionamento: N Produtos → 1 Categoria.
    /// </summary>
    public Categoria? Categoria { get; set; }

    /// <summary>
    /// Coleção de itens de pedido que referenciam este produto.
    /// Relacionamento: 1 Produto aparece em N ItensPedido.
    /// </summary>
    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();

}
