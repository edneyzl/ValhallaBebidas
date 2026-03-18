using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Application.DTOs
{
    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CriarPedidoDto
    {
   
        /// <summary>Chave estrangeira para o pedido</summary>
        public int PedidoId { get; set; }
        /// <summary>Chave estrangeira para o produto</summary>
        public int ProdutoId { get; set; }
        /// <summary>Quantidade deste produto no pedido</summary>
        public int Quantidade { get; set; }
        //calculo dentro de uma classe - não é boa e nem má pratica - á tipico
        public decimal Subtotal => Quantidade * Produto.PrecoVenda; //corrigir proxima aula


    }
}
