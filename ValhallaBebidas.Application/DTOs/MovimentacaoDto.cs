using ValhallaBebidas.Domain.Enums;
namespace ValhallaBebidas.Application.DTOs;



public class MovimentacaoDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public DirecaoMovimentacao Direcao { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Impacto { get; set; } /* calculado pelo backend */
}

public class CriarMovimentacaoDto
{
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public DirecaoMovimentacao Direcao { get; set; }
    public string Motivo { get; set; } = string.Empty;
    /* Data e Impacto gerados pelo backend */
}

public class AtualizarMovimentacaoDto
{
    /* Id vem pela URL — não precisa no body */
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public DirecaoMovimentacao Direcao { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
