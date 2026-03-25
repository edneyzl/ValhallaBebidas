using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<Categoria?> ObterPorNomeAsync(string nome);
        Task<IEnumerable<Categoria>> ListarTodosAsync();
        Task AdicionarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task RemoverAsync(int id);
    }
}
