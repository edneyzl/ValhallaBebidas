namespace ValhallaBebidas.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Documento { get; set; } = string.Empty;//CPF CNPJ
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty; //vai ter criptografia, questão da LGPD
    public bool Status { get; set; } = true; //ativo por padrão
    public int? EnderecoId { get; set; }

    //Propriedade de navegação, referência entre entidades, possui o tipo da classe
    public Endereco? Endereco { get; set; }

    /// <summary>
    /// Coleção de pedidos associados a este cliente.
    /// Relacionamento: 1 Cliente possui N Pedidos.
    /// </summary>
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
