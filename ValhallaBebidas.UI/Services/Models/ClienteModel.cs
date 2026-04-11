namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelhos locais dos DTOs de Cliente da ValhallaBebidas.API.
    /// A UI não referencia o projeto Application — esses POCOs são usados
    /// para desserializar as respostas JSON da API.
    /// </summary>

    // Resposta de GET /api/cliente  e  GET /api/cliente/{id}
    public class ClienteDto
    {
        public int    Id   { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF  { get; set; } = string.Empty;
    }

    // Corpo de POST /api/cliente
    public class CriarClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF  { get; set; } = string.Empty;
    }

    // Corpo de PUT /api/cliente/{id}
    public class AtualizarClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF  { get; set; } = string.Empty;
    }
}
