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
    public int QuantidadeEstoque { get; set; }
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Status { get; set; }
    public int CategoriaId { get; set; }
    public string? FotoProduto { get; set; } 

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 
    public Categoria? Categoria { get; set; }

}
