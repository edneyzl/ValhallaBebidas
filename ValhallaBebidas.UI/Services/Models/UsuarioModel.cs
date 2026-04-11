namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelhos locais dos DTOs de Usuário da ValhallaBebidas.API.
    /// </summary>

    // Resposta de GET /api/usuario  e  PUT /api/usuario/{id}
    public class UsuarioDto
    {
        public int    Id         { get; set; }
        public string Nome       { get; set; } = string.Empty;
        public string Email      { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
    }

    // Corpo de POST /api/usuario
    public class CriarUsuarioDto
    {
        public string Nome  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
    }

    // Corpo de POST /api/usuario/login
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    // Resposta de POST /api/usuario/login
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
