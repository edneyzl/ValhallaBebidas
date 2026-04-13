namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelhos locais dos DTOs de funcionario da ValhallaBebidas.API.
    /// </summary>

    // Resposta de GET /api/funcionario  e  PUT /api/funcionario/{id}
    public class FuncionarioDto
    {
        public int    Id         { get; set; }
        public string Nome       { get; set; } = string.Empty;
        public string Email      { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
    }

    // Corpo de POST /api/funcionario
    public class CriarFuncionarioDto
    {
        public string Nome  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DataNascimento { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
    }

    // Corpo de POST /api/funcionario/login
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    // Resposta de POST /api/funcionario/login
    public class LoginResponseDto
    {
        public int    Id       { get; set; }
        public string Nome     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public bool   Sucesso  { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
    }
}
