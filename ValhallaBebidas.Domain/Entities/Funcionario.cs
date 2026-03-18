namespace ValhallaBebidas.Domain.Entities;

public class Funcionario
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public bool Status { get; set; }
    public int EnderecoId { get; set; }//chave estrangeira para o endereço do funcionário, obrigatório, um funcionário deve ter um endereço

    //Propriedade de navegação, referência entre entidades, possui o tipo da classe
    public Endereco? Endereco { get; set; }
}
