using System.Numerics;

namespace ValhallaBebidas.Domain.Entities;

public class Produto
{

    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string EANCodBarras { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }

    public int QuantidadeEstoque { get; set; }
    public int QuantidadeMinimo { get; set; }

    public DateTime DataCadastro { get; set; }

    public string Status { get; set; } = string.Empty;

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 

    public Categoria? Categoria { get; set; }






}
