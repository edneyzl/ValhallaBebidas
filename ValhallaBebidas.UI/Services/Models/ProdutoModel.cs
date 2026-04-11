namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelhos locais dos DTOs de Produto da ValhallaBebidas.API.
    /// </summary>

    // Resposta de GET /api/produto  e  GET /api/produto/{id}
    public class ProdutoDto
    {
        public int     Id            { get; set; }
        public string  Nome          { get; set; } = string.Empty;
        public decimal Preco         { get; set; }
        public string? FotoProduto   { get; set; }
        public int?    CategoriaId   { get; set; }
        public string? NomeCategoria { get; set; }
    }

    // Corpo de POST /api/produto
    public class CriarProdutoDto
    {
        public string  Nome        { get; set; } = string.Empty;
        public decimal Preco       { get; set; }
        public string? FotoProduto { get; set; }
        public int?    CategoriaId { get; set; }
    }

    // Corpo de PUT /api/produto/{id}
    public class AtualizarProdutoDto
    {
        public string  Nome        { get; set; } = string.Empty;
        public decimal Preco       { get; set; }
        public string? FotoProduto { get; set; }
        public int?    CategoriaId { get; set; }
    }
}
