namespace ValhallaBebidas.Application.DTOs;

public class ProdutoDto
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
    public int CategoriaId { get; set; }
    public string? FotoProduto { get; set; }
}

public class CriarProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public string EanCodBarras { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; }
    public int CategoriaId { get; set; }
    public string? FotoProduto { get; set; }
}

public class AtualizarProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public string EanCodBarras { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Status { get; set; }
    public int CategoriaId { get; set; }
    public string? FotoProduto { get; set; }
}
