namespace ValhallaBebidas.UI.DTO;

public class CepModel
{
    public string cep { get; set; } = string.Empty;
    public string? logradouro { get; set; }
    public string bairro { get; set; } = string.Empty;
    public string localidade { get; set; } = string.Empty;
    public string uf { get; set; } = string.Empty;
}
