namespace ValhallaBebidas.Application.DTOs;

public class FuncionarioDto
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public bool Status { get; set; }
    /* SenhaHash removido — nunca retornar senha na resposta */
}

public class CriarFuncionarioDto
{
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; /* backend faz o hash */
    public CriarEnderecoDto Endereco { get; set; } = new();
}

public class AtualizarFuncionarioDto
{
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; /* backend faz o hash */
    public bool Status { get; set; }
}

public class LoginFuncionarioDto
{
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>Resposta após login bem-sucedido do funcionário</summary>
public class LoginFuncionarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
