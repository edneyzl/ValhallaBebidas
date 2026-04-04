namespace ValhallaBebidas.Application.DTOs;

public class CheckoutPayload
{
    public List<CheckoutItem> Itens { get; set; } = new();
    public EnderecoEntrega Entrega { get; set; } = new();
}

public class CheckoutItem
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Imagem { get; set; } = string.Empty;
}

public class EnderecoEntrega
{
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
}
