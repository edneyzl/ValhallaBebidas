using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<Cliente?> ObterPorEmailAsync(string email);
        Task<Cliente?> ObterPorDocumentoAsync(string documento);
        Task<IEnumerable<Cliente>> ListarTodosAsync();
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(int id);
        Task SaveAsync();
    }
}
