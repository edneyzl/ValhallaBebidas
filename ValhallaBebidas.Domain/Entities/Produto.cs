using System.Numerics;
using System.ComponentModel.DataAnnotations;

namespace ValhallaBebidas.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeEstoque { get; set; } /* ← adicionado — estoque atual */
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now; /* ← valor padrão */
    public bool Status { get; set; } = true;         /* ← produto já começa ativo */

    /// <summary>
    /// Chave estrangeira para a categoria do produto.
    /// Opcional — produto pode existir sem categoria.
    /// </summary>
    public int? CategoriaId { get; set; } /* ← nullable, comentário dizia opcional */

    /// <summary>
    /// Caminho relativo para a foto do produto.
    /// Opcional, salvo dentro de uploads/produtos.
    /// </summary>
    public string? FotoProduto { get; set; }

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
