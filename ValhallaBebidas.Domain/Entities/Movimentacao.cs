using System;
using System.Collections.Generic;
using System.Text;

namespace ValhallaBebidas.Domain.Entities
{
    public class Movimentacao
    {
        public int Id { get; set; } // Identificador único
        public int ProdutoId { get; set; } // ID do produto que está movendo
        public decimal Quantidade { get; set; } // Valor absoluto (ex: 10)
        public int Direcao { get; set; } // Lógica substituindo o Enum: // 1 = Entrada (Soma), -1 = Saída/Perda (Subtrai)
        public string Motivo { get; set; } = string.Empty; // Ex: "Venda", "Quebra"
        public DateTime Data { get; set; } = DateTime.Now; // Data da operação

        //Propriedade de navegação, referência entre entidades, possui o tipo da classe
        public Produto? Produto { get; set; }

        // Propriedade calculada: retorna o valor real para o cálculo de saldo
        // Se Direcao for -1 e Quantidade 10, retorna -10.
        public decimal ValorImpactoEstoque => Quantidade * Direcao;
    }
}
