namespace ValhallaBebidas.Application.DTOs;

public class MovimentacaoDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; } 
    public decimal Quantidade { get; set; } 
    public int Direcao { get; set; } 
    public string Motivo { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal ValorImpactoEstoque => Quantidade * Direcao;
}

public class CriarMovimentacaoDto
{
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public int Direcao { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal ValorImpactoEstoque => Quantidade * Direcao;
}

public class AtualizarMovimentacaoDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public int Direcao { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal ValorImpactoEstoque => Quantidade * Direcao;
}

