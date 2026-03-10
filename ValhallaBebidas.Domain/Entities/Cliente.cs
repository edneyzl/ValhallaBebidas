namespace ValhallaBebidas.Domain.Entities;

public class Cliente
{

    public int Id { get; set; }

    public string NomeCliente { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    public string Documento { get; set; } = string.Empty;//CPF CNPJ

    public string Telefone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string login { get; set; } = string.Empty;


    public string SenhaHash { get; set; } = string.Empty; //vai ter criptografia, questão da LGPD





}
