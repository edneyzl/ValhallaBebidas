namespace ValhallaBebidas.Application.DTOs;

public class AlterarStatusDto
{
    public bool Status { get; set; }
}

public class AtualizarStatusDto
{
    public string NovoStatus { get; set; } = string.Empty;
}