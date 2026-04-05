using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Domain.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; } // Identificador único
        public int ProdutoId { get; set; } // ID do produto que está movendo
        public int Quantidade { get; set; } // Valor absoluto (ex: 10)
        public DirecaoMovimentacao Direcao { get; set; } // Lógica substituindo o Enum: // 1 = Entrada (Soma), -1 = Saída/Perda (Subtrai)
        public string Motivo { get; set; } = string.Empty; // Ex: "Venda", "Quebra"
        public DateTime Data { get; set; } = DateTime.UtcNow; // Data da operação (UTC)

        //Propriedade de navegação, referência entre entidades, possui o tipo da classe
        public Produto? Produto { get; set; }

        public decimal ValorImpactoEstoque => Direcao == DirecaoMovimentacao.Entrada ? Quantidade : -Quantidade;
    }
}
