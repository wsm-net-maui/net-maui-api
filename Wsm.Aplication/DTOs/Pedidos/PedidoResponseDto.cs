using Wsm.Domain.Enums;

namespace Wsm.Aplication.DTOs.Pedidos;

public class PedidoResponseDto
{
    public Guid Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public StatusPedido Status { get; set; }
    public string StatusDescricao { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public List<PedidoItemResponseDto> Itens { get; set; } = new();
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
