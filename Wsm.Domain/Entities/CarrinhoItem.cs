namespace Wsm.Domain.Entities;

public class CarrinhoItem : BaseEntity
{
    public Guid CarrinhoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public Carrinho Carrinho { get; private set; } = null!;
    public Produto Produto { get; private set; } = null!;

    private CarrinhoItem() { }

    public CarrinhoItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        ValidarQuantidade(quantidade);
        ValidarPreco(precoUnitario);

        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public void AtualizarQuantidade(int quantidade)
    {
        ValidarQuantidade(quantidade);
        Quantidade = quantidade;
        AtualizarDataModificacao();
    }

    public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;

    private static void ValidarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));
    }

    private static void ValidarPreco(decimal precoUnitario)
    {
        if (precoUnitario < 0)
            throw new ArgumentException("Preço unitário não pode ser negativo", nameof(precoUnitario));
    }
}
