namespace Wsm.Aplication.DTOs.Produtos;

public class CriarProdutoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? CodigoBarras { get; set; }
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    public int EstoqueMinimo { get; set; }
    public Guid CategoriaId { get; set; }
}
