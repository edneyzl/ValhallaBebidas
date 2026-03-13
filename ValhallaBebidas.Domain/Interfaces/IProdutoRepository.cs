using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorIdAsync(int id);
        Task<IEnumerable<Produto>> ListarTodosAsync();
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(int id);
    }
}
