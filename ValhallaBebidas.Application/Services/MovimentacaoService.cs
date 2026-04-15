using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class MovimentacaoService
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public MovimentacaoService(
        IMovimentacaoRepository movimentacaoRepository,
        IProdutoRepository produtoRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<MovimentacaoDto>> ListarTodosAsync()
    {
        var movimentacoes = await _movimentacaoRepository.ListarTodosAsync();
        return movimentacoes.Select(MapearParaDto);
    }

    public async Task<IEnumerable<MovimentacaoDto>> ListarPorProdutoAsync(int produtoId)
    {
        var movimentacoes = await _movimentacaoRepository.ListarPorProdutoAsync(produtoId);
        return movimentacoes.Select(MapearParaDto);
    }

    public async Task<MovimentacaoDto?> ObterPorIdAsync(int id)
    {
        var movimentacao = await _movimentacaoRepository.ObterPorIdAsync(id);
        return movimentacao == null ? null : MapearParaDto(movimentacao);
    }

    public async Task<MovimentacaoDto> RegistrarEntradaAsync(CriarMovimentacaoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {dto.ProdutoId} não encontrado.");

        var quantidade = (int)dto.Quantidade;
        if (quantidade <= 0)
            throw new InvalidOperationException("A quantidade de entrada deve ser maior que zero.");

        var movimentacao = new Movimentacao
        {
            ProdutoId = dto.ProdutoId,
            Quantidade = quantidade,
            Direcao = DirecaoMovimentacao.Entrada,
            Motivo = dto.Motivo,
            Data = DateTime.UtcNow,
        };

        produto.QuantidadeEstoque += quantidade;
        await _produtoRepository.AtualizarAsync(produto);

        await _movimentacaoRepository.AdicionarAsync(movimentacao);

        await _produtoRepository.SaveAsync();

        var salva = await _movimentacaoRepository.ObterPorIdAsync(movimentacao.Id);
        return MapearParaDto(salva!);
    }

    public async Task<MovimentacaoDto> RegistrarSaidaAsync(CriarMovimentacaoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {dto.ProdutoId} não encontrado.");

        var quantidade = dto.Quantidade;
        if (quantidade <= 0)
            throw new InvalidOperationException("A quantidade de saída deve ser maior que zero.");

        if (produto.QuantidadeEstoque < quantidade)
            throw new InvalidOperationException(
                $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}.");

        var movimentacao = new Movimentacao
        {
            ProdutoId = dto.ProdutoId,
            Quantidade = quantidade,
            Direcao = DirecaoMovimentacao.Saida,
            Motivo = dto.Motivo,
            Data = DateTime.UtcNow,
        };

        produto.QuantidadeEstoque -= quantidade;
        await _produtoRepository.AtualizarAsync(produto);

        await _movimentacaoRepository.AdicionarAsync(movimentacao);

        await _produtoRepository.SaveAsync();

        var salva = await _movimentacaoRepository.ObterPorIdAsync(movimentacao.Id);
        return MapearParaDto(salva!);
    }

    public async Task EstornarAsync(int id)
    {
        var movimentacao = await _movimentacaoRepository.ObterPorIdAsync(id);
        if (movimentacao == null)
            throw new KeyNotFoundException($"Movimentacao com Id {id} não encontrada.");

        var produto = await _produtoRepository.ObterPorIdAsync(movimentacao.ProdutoId);
        if (produto == null)
            return;

        if (movimentacao.Direcao == DirecaoMovimentacao.Entrada)
        {
            if (produto.QuantidadeEstoque < movimentacao.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque atual ({produto.QuantidadeEstoque}) é insuficiente para estornar {movimentacao.Quantidade} unidade(s) de '{produto.Nome}'.");
            produto.QuantidadeEstoque -= movimentacao.Quantidade;
        }
        else
        {
            produto.QuantidadeEstoque += movimentacao.Quantidade;
        }

        await _produtoRepository.AtualizarAsync(produto);
        await _produtoRepository.SaveAsync();
    }

    public async Task RemoverAsync(int id)
    {
        await _movimentacaoRepository.RemoverAsync(id);
        await _movimentacaoRepository.SaveAsync();
    }

    private static MovimentacaoDto MapearParaDto(Movimentacao m) => new()
    {
        Id = m.Id,
        ProdutoId = m.ProdutoId,
        NomeProduto = m.Produto?.Nome ?? string.Empty,
        Quantidade = m.Quantidade,
        Direcao = m.Direcao,
        Motivo = m.Motivo,
        Data = m.Data,
        Impacto = m.ValorImpactoEstoque,
    };
}