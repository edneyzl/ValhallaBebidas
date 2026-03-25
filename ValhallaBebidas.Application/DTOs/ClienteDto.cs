namespace ValhallaBebidas.Application.DTOs;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
}

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

public class AtualizarClienteDto
{
    public string Nome { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
}

public class LoginClienteDto
{
    public string Email { get; set; } = string.Empty; /* usa email como login */
    public string Senha { get; set; } = string.Empty;
}

public class LoginClienteResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; /* identificador do cliente */
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
