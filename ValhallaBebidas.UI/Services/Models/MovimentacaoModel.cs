using System;
using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelhos locais dos DTOs de Movimentação da ValhallaBebidas.API.
    /// </summary>

    // Resposta de GET /api/movimentacao e GET /api/movimentacao/{id}
    public class MovimentacaoDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public DirecaoMovimentacao Direcao { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal Impacto { get; set; } // calculado no backend
    }

    // Corpo de POST /api/movimentacao/entrada e /saida
    public class CriarMovimentacaoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public DirecaoMovimentacao Direcao { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    // (Opcional) caso futuramente tenha update
    public class AtualizarMovimentacaoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public DirecaoMovimentacao Direcao { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}