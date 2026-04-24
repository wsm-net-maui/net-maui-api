namespace Wsm.Aplication.DTOs.Carrinhos;

public class AdicionarItemCarrinhoRequestDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
