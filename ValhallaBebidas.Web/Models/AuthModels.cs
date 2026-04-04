using System.ComponentModel.DataAnnotations;

namespace ValhallaBebidas.Web.Models;

public class LoginFormModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;
}

public class CadastroFormModel
{
    [Required] public string Nome { get; set; } = string.Empty;
    [Required] public string DataNascimento { get; set; } = string.Empty;
    [Required] public string Documento { get; set; } = string.Empty;
    [Required] public string Telefone { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    [Required][MinLength(6)] public string Senha { get; set; } = string.Empty;
    public EnderecoModel Endereco { get; set; } = new();
}

public class EnderecoModel
{
    public string Logradouro { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}

public class LoginResponseModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}