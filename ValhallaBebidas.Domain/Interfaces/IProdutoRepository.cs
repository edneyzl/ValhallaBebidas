using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorIdAsync(int id);
        Task<Produto?> ObterPorEanAsync(string ean);
        Task<IEnumerable<Produto>> ObterPorIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Produto>> ListarTodosAsync();
        Task<IEnumerable<Produto>> ListarPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> ObterEstoqueBaixoAsync();
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(int id);
    }
}
