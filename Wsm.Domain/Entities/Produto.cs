namespace Wsm.Domain.Entities;

public class Produto : BaseEntity
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public string? CodigoBarras { get; private set; }
    public decimal PrecoCompra { get; private set; }
    public decimal PrecoVenda { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public int EstoqueMinimo { get; private set; }
    public Guid CategoriaId { get; private set; }

    public Categoria Categoria { get; private set; } = null!;

    private readonly List<MovimentacaoEstoque> _movimentacoes = new();
    public IReadOnlyCollection<MovimentacaoEstoque> Movimentacoes => _movimentacoes.AsReadOnly();

    private Produto() { }

    public Produto(
        string nome, 
        decimal precoCompra, 
        decimal precoVenda, 
        int estoqueMinimo,
        Guid categoriaId,
        string? descricao = null,
        string? codigoBarras = null)
    {
        ValidarNome(nome);
        ValidarPrecos(precoCompra, precoVenda);
        ValidarEstoqueMinimo(estoqueMinimo);

        Nome = nome;
        Descricao = descricao;
        CodigoBarras = codigoBarras;
        PrecoCompra = precoCompra;
        PrecoVenda = precoVenda;
        QuantidadeEstoque = 0;
        EstoqueMinimo = estoqueMinimo;
        CategoriaId = categoriaId;
    }

    public void AtualizarDados(
        string nome, 
        decimal precoCompra, 
        decimal precoVenda, 
        int estoqueMinimo,
        Guid categoriaId,
        string? descricao = null,
        string? codigoBarras = null)
    {
        ValidarNome(nome);
        ValidarPrecos(precoCompra, precoVenda);
        ValidarEstoqueMinimo(estoqueMinimo);

        Nome = nome;
        Descricao = descricao;
        CodigoBarras = codigoBarras;
        PrecoCompra = precoCompra;
        PrecoVenda = precoVenda;
        EstoqueMinimo = estoqueMinimo;
        CategoriaId = categoriaId;
        AtualizarDataModificacao();
    }

    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));

        QuantidadeEstoque += quantidade;
        AtualizarDataModificacao();
    }

    public void RemoverEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));

        if (quantidade > QuantidadeEstoque)
            throw new InvalidOperationException($"Estoque insuficiente. Disponível: {QuantidadeEstoque}, Solicitado: {quantidade}");

        QuantidadeEstoque -= quantidade;
        AtualizarDataModificacao();
    }

    public bool EstaAbaixoEstoqueMinimo() => QuantidadeEstoque < EstoqueMinimo;

    public decimal CalcularMargemLucro()
    {
        if (PrecoCompra == 0)
            return 0;

        return ((PrecoVenda - PrecoCompra) / PrecoCompra) * 100;
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do produto é obrigatório", nameof(nome));

        if (nome.Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 caracteres", nameof(nome));

        if (nome.Length > 100)
            throw new ArgumentException("Nome deve ter no máximo 100 caracteres", nameof(nome));
    }

    private static void ValidarPrecos(decimal precoCompra, decimal precoVenda)
    {
        if (precoCompra < 0)
            throw new ArgumentException("Preço de compra não pode ser negativo", nameof(precoCompra));

        if (precoVenda < 0)
            throw new ArgumentException("Preço de venda não pode ser negativo", nameof(precoVenda));

        if (precoVenda < precoCompra)
            throw new ArgumentException("Preço de venda não pode ser menor que o preço de compra");
    }

    private static void ValidarEstoqueMinimo(int estoqueMinimo)
    {
        if (estoqueMinimo < 0)
            throw new ArgumentException("Estoque mínimo não pode ser negativo", nameof(estoqueMinimo));
    }
}
