using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class MovimentacaoService
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MovimentacaoService(
        IMovimentacaoRepository movimentacaoRepository,
        IProdutoRepository produtoRepository,
        IUnitOfWork unitOfWork)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    // ════════════════════════════════════════
    // LISTAR TODAS — com nome do produto
    // ════════════════════════════════════════
    public async Task<IEnumerable<MovimentacaoDto>> ListarTodosAsync()
    {
        var movimentacoes = await _movimentacaoRepository.ListarTodosAsync();
        return movimentacoes.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // LISTAR POR PRODUTO — extrato do item
    // ════════════════════════════════════════
    public async Task<IEnumerable<MovimentacaoDto>> ListarPorProdutoAsync(int produtoId)
    {
        var movimentacoes = await _movimentacaoRepository.ListarPorProdutoAsync(produtoId);
        return movimentacoes.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // OBTER POR ID
    // ════════════════════════════
    public async Task<MovimentacaoDto?> ObterPorIdAsync(int id)
    {
        var movimentacao = await _movimentacaoRepository.ObterPorIdAsync(id);
        return movimentacao == null ? null : MapearParaDto(movimentacao);
    }

    // ════════════════════════════════════════
    // REGISTRAR ENTRADA — compra, ajuste positivo, devolucao
    // ════════════════════════════════════════
    public async Task<MovimentacaoDto> RegistrarEntradaAsync(CriarMovimentacaoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {dto.ProdutoId} nao encontrado.");

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

        /* Atualiza o estoque do produto no mesmo transaction scope */
        produto.QuantidadeEstoque += quantidade;
        await _produtoRepository.AtualizarAsync(produto);

        await _movimentacaoRepository.AdicionarAsync(movimentacao);
        await _unitOfWork.SaveChangesAsync();

        return MapearParaDto(movimentacao);
    }

    // ════════════════════════════════════════
    // REGISTRAR SAIDA — venda, perda, ajuste negativo
    // ════════════════════════════════════════
    public async Task<MovimentacaoDto> RegistrarSaidaAsync(CriarMovimentacaoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {dto.ProdutoId} nao encontrado.");

        var quantidade = dto.Quantidade;
        if (quantidade <= 0)
            throw new InvalidOperationException("A quantidade de saida deve ser maior que zero.");

        if (produto.QuantidadeEstoque < quantidade)
            throw new InvalidOperationException(
                $"Estoque insuficiente para '{produto.Nome}'. Disponivel: {produto.QuantidadeEstoque}.");

        var movimentacao = new Movimentacao
        {
            ProdutoId = dto.ProdutoId,
            Quantidade = quantidade,
            Direcao = DirecaoMovimentacao.Saida,
            Motivo = dto.Motivo,
            Data = DateTime.UtcNow,
        };

        /* Decrementa o estoque do produto no mesmo transaction scope */
        produto.QuantidadeEstoque -= quantidade;
        await _produtoRepository.AtualizarAsync(produto);

        await _movimentacaoRepository.AdicionarAsync(movimentacao);
        await _unitOfWork.SaveChangesAsync();

        return MapearParaDto(movimentacao);
    }

    // ════════════════════════════════════════
    // ESTORNAR — reverte uma movimentacao existente
    //   Entrada → vira Saida (subtrai do estoque)
    //   Saida   → vira Entrada (soma ao estoque)
    // ════════════════════════════════════════
    public async Task EstornarAsync(int id)
    {
        var movimentacao = await _movimentacaoRepository.ObterPorIdAsync(id);
        if (movimentacao == null)
            throw new KeyNotFoundException($"Movimentacao com Id {id} nao encontrada.");

        var produto = await _produtoRepository.ObterPorIdAsync(movimentacao.ProdutoId);
        /* Movimentacao sem produto vinculado = inconsistencia; nada a estornar */
        if (produto == null)
            return;

        if (movimentacao.Direcao == DirecaoMovimentacao.Entrada)
        {
            /* Entrou antes → agora precisa sair */
            if (produto.QuantidadeEstoque < movimentacao.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque atual ({produto.QuantidadeEstoque}) e insuficiente para estornar {movimentacao.Quantidade} unidade(s) de '{produto.Nome}'.");

            produto.QuantidadeEstoque -= movimentacao.Quantidade;
        }
        else
        {
            /* Saiu antes → agora precisa voltar */
            produto.QuantidadeEstoque += movimentacao.Quantidade;
        }

        await _produtoRepository.AtualizarAsync(produto);
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // REMOVER — apagar registro de movimentacao
    // Nao reverte saldo automaticamente — use EstornarAsync para isso.
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        await _movimentacaoRepository.RemoverAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // MAPPER — Entidade → DTO
    // ════════════════════════════════════════
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
