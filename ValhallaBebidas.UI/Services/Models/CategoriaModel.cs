namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// DTO local que espelha a resposta do endpoint GET /api/categorias.
    /// </summary>
    public class CategoriaDto
    {
        public int    Id   { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
