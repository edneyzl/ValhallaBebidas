using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<Endereco?> ObterPorIdAsync(int id);
        Task<Endereco?> ObterPorCepAsync(string cep);
        Task AdicionarAsync(Endereco endereco);
        Task AtualizarAsync(Endereco endereco);
        Task RemoverAsync(int id);
    }
}
