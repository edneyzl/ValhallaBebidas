namespace ValhallaBebidas.Application.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeEstoque { get; set; } /* adicionado — frontend precisa saber */
    public int QuantidadeMinimo { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Status { get; set; }
    public int CategoriaId { get; set; }
    public string NomeCategoria { get; set; } = string.Empty; /* útil para exibir */
    public string? FotoProduto { get; set; }
}

public class CriarProdutoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeMinimo { get; set; }
    public int? CategoriaId { get; set; }
    public string? FotoProduto { get; set; }
}

public class AtualizarProdutoDto
{
    /* Id vem pela URL — não precisa no body */
    public string Nome { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCusto { get; set; }
    public int QuantidadeMinimo { get; set; }
    public bool Status { get; set; }
    public int CategoriaId { get; set; }
    public string? FotoProduto { get; set; }
}
