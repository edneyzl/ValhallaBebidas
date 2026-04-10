using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Application.DTOs;

public class AtualizarStatusDto
{
    public StatusPedido NovoStatus { get; set; }
}