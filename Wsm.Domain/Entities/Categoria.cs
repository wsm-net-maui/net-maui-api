namespace Wsm.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }

    private readonly List<Produto> _produtos = new();
    public IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

    private Categoria() { }

    public Categoria(string nome, string? descricao = null)
    {
        ValidarNome(nome);
        
        Nome = nome;
        Descricao = descricao;
    }

    public void AtualizarDados(string nome, string? descricao)
    {
        ValidarNome(nome);
        
        Nome = nome;
        Descricao = descricao;
        AtualizarDataModificacao();
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da categoria é obrigatório", nameof(nome));

        if (nome.Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 caracteres", nameof(nome));

        if (nome.Length > 50)
            throw new ArgumentException("Nome deve ter no máximo 50 caracteres", nameof(nome));
    }
}
