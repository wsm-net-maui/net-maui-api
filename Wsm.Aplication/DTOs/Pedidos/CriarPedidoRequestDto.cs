namespace Wsm.Aplication.DTOs.Pedidos;

public class CriarPedidoRequestDto
{
    public List<CriarPedidoItemRequestDto> Itens { get; set; } = new();
}
