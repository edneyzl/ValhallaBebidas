using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class ProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;


    public ProdutoService(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;

    }

    // ════════════════════════════════════════
    // LISTAR TODOS
    // ════════════════════════════════════════
    public async Task<IEnumerable<ProdutoDto>> ListarTodosAsync()
    {
        var produtos = await _produtoRepository.ListarTodosAsync();
        return produtos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // LISTAR ATIVOS — usado pelo frontend/catalogo + dashboard
    //    Filtra por Status=true diretamente no banco (WHERE).
    // ════════════════════════════════════════
    public async Task<IEnumerable<ProdutoDto>> ListarAtivosAsync()
    {
        var produtos = await _produtoRepository.ListarAtivosAsync();
        return produtos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // LISTAR POR CATEGORIA
    // ════════════════════════════════════════
    public async Task<IEnumerable<ProdutoDto>> ListarPorCategoriaAsync(int categoriaId)
    {
        var produtos = await _produtoRepository.ListarPorCategoriaAsync(categoriaId);
        return produtos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // OBTER POR ID
    // ════════════════════════════════════════
    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        return produto == null ? null : MapearParaDto(produto);
    }

    // ════════════════════════════════════════
    // CRIAR
    // ════════════════════════════════════════
    public async Task<ProdutoDto> CriarAsync(CriarProdutoDto dto)
    {
        /* Preço não pode ser negativo */
        if (dto.PrecoVenda < 0)
            throw new InvalidOperationException("O preço de venda não pode ser negativo.");

        if (dto.PrecoCusto < 0)
            throw new InvalidOperationException("O preço de custo não pode ser negativo.");

        /* EAN único */
        var existenteEan = await _produtoRepository.ObterPorEanAsync(dto.Ean);
        if (existenteEan != null)
            throw new InvalidOperationException($"Já existe um produto com o EAN '{dto.Ean}'.");

        /* Categoria existe */
        if (dto.CategoriaId.HasValue)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync(dto.CategoriaId.Value);
            if (categoria == null)
                throw new KeyNotFoundException($"Categoria com Id {dto.CategoriaId} não encontrada.");
        }

        var produto = new Produto
        {
            Nome = dto.Nome,
            Ean = dto.Ean,
            Descricao = dto.Descricao,
            PrecoVenda = dto.PrecoVenda,
            PrecoCusto = dto.PrecoCusto,
            QuantidadeMinimo = dto.QuantidadeMinimo,
            QuantidadeEstoque = 0, /* estoque inicial zerado — entra via Movimentacao */
            DataCadastro = DateTime.UtcNow,
            Status = true,
            CategoriaId = dto.CategoriaId,
            FotoProduto = dto.FotoProduto,
        };

        await _produtoRepository.AdicionarAsync(produto);


        return MapearParaDto(produto);
    }

    // ════════════════════════════════════════
    // ATUALIZAR
    // ════════════════════════════════════════
    public async Task AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        if (dto.PrecoVenda < 0)
            throw new InvalidOperationException("O preço de venda não pode ser negativo.");

        if (dto.PrecoCusto < 0)
            throw new InvalidOperationException("O preço de custo não pode ser negativo.");

        /* Verifica EAN duplicado em outro produto */
        var comMesmoEan = await _produtoRepository.ObterPorEanAsync(dto.Ean);
        if (comMesmoEan != null && comMesmoEan.Id != id)
            throw new InvalidOperationException($"O EAN '{dto.Ean}' já está em uso por outro produto.");

        produto.Nome = dto.Nome;
        produto.Ean = dto.Ean;
        produto.Descricao = dto.Descricao;
        produto.PrecoVenda = dto.PrecoVenda;
        produto.PrecoCusto = dto.PrecoCusto;
        produto.QuantidadeMinimo = dto.QuantidadeMinimo;
        produto.Status = dto.Status;
        produto.CategoriaId = dto.CategoriaId;

        if (!string.IsNullOrWhiteSpace(dto.FotoProduto))
            produto.FotoProduto = dto.FotoProduto;

        await _produtoRepository.AtualizarAsync(produto);

    }

    // ════════════════════════════════════════
    // ATIVAR / INATIVAR — soft delete
    // ════════════════════════════════════════
    public async Task AlterarStatusAsync(int id, bool status)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        produto.Status = status;
        await _produtoRepository.AtualizarAsync(produto);

    }

    // ════════════════════════════════════════
    // REMOVER
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        await _produtoRepository.RemoverAsync(id);

    }

    // ════════════════════════════════════════
    // VERIFICAR ESTOQUE BAIXO — Windows Forms dashboard
    // ════════════════════════════════════════
    public async Task<IEnumerable<ProdutoDto>> ListarEstoqueBaixoAsync()
    {
        var produtos = await _produtoRepository.ObterEstoqueBaixoAsync();
        return produtos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // MAPPER — Entidade → DTO
    // ════════════════════════════════════════
    private static ProdutoDto MapearParaDto(Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Ean = p.Ean,
        Descricao = p.Descricao,
        PrecoVenda = p.PrecoVenda,
        PrecoCusto = p.PrecoCusto,
        QuantidadeEstoque = p.QuantidadeEstoque,
        QuantidadeMinimo = p.QuantidadeMinimo,
        DataCadastro = p.DataCadastro,
        Status = p.Status,
        CategoriaId = p.CategoriaId ?? 0,
        NomeCategoria = p.Categoria?.Nome ?? string.Empty,
        FotoProduto = p.FotoProduto,
    };
}