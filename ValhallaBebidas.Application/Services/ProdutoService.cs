using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class ProdutoService
{

    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }


    public async Task<IEnumerable<ProdutoDto>> ListarTodosAsync()
    {
        var produtos = await _produtoRepository.ListarTodosAsync();
        return produtos.Select(p => new ProdutoDto
        {
            Id = p.Id,
            Nome = p.Nome,
            PrecoVenda = p.PrecoVenda,
            FotoProduto = p.FotoProduto
        });
    }


    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null) return null;

        return new ProdutoDto { Id = produto.Id, Nome = produto.Nome, PrecoVenda = produto.PrecoVenda, FotoProduto = produto.FotoProduto };
    }



    public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
    {
        // Regra de negócio: preço não pode ser negativo
        if (dto.PrecoVenda < 0)
            throw new InvalidOperationException("O preço do produto não pode ser negativo.");

        var produto = new Produto
        {
            Nome = dto.Nome,
            PrecoVenda = dto.PrecoVenda,
            FotoProduto = dto.FotoProduto
        };

        await _produtoRepository.AdicionarAsync(produto);

        return new ProdutoDto { Id = produto.Id, Nome = produto.Nome, PrecoVenda = produto.PrecoVenda, FotoProduto = produto.FotoProduto };
    }


    public async Task AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        // Regra de negócio: preço não pode ser negativo
        if (dto.PrecoVenda < 0)
            throw new InvalidOperationException("O preço do produto não pode ser negativo.");

        produto.Nome = dto.Nome;
        produto.PrecoVenda = dto.PrecoVenda;
        if (dto.FotoProduto != null)
        {
            produto.FotoProduto = dto.FotoProduto;
        }

        await _produtoRepository.AtualizarAsync(produto);
    }


    public async Task RemoverAsync(int id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        await _produtoRepository.RemoverAsync(id);
    }


}
