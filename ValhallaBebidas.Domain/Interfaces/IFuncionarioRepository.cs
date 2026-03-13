using System;
using System.Collections.Generic;
using System.Text;
using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces
{
    public interface IFuncionarioRepository
    {
        //operações assincrona <Tipo/retorno do método/tipagem>
        Task<Funcionario?> ObterPorIdAsync(int id);//método para obter um usuário por id, retornando um objeto do tipo Usuario ou null se não encontrado

        Task<Funcionario?> ObterPorCpfAsync(string cpf);//método para obter um usuário por email, retornando um objeto do tipo Usuario ou null se não encontrado
        
        Task<Funcionario?> ObterPorNomeAsync(string nome);//método para obter um usuário por email, retornando um objeto do tipo Usuario ou null se não encontrado
        
        //Task<Funcionario?> ObterPorCargoAsync (string cargo);

        Task<IEnumerable<Funcionario?>> ListarTodosAsync();//método para listar todos os usuários, retornando uma lista de objetos do tipo Usuario

        Task AdicionarAsync(Funcionario funcionario);//método para adicionar um novo usuário, recebendo um objeto do tipo Usuario e retornando a tarefa assíncrona

        Task AtualizarAsync(Funcionario funcionario);//método para atualizar um usuário existente, recebendo um objeto do tipo Usuario e retornando a tarefa assíncrona

        Task RemoverAsync(int id);
    }
}

