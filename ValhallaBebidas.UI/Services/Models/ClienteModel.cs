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
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public bool Status { get; set; }
        public EnderecoDto? Endereco { get; set; }
    }

    // Corpo de POST /api/cliente
    public class CriarClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty; /* CPF/CNPJ */
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public CriarEnderecoDto Endereco { get; set; } = new();
    }


    // Corpo de PUT /api/cliente/{id}
    public class AtualizarClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
    }

    public class AtualizarSenhaDto
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
