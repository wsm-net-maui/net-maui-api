namespace Wsm.Aplication.DTOs.Pedidos;

public class PedidoItemResponseDto
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
