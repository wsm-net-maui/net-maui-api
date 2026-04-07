using Wsm.Domain.Enums;

namespace Wsm.Domain.Entities;

public class MovimentacaoEstoque : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public TipoMovimentacao Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public string? Observacao { get; private set; }
    public Guid UsuarioId { get; private set; }

    public Produto Produto { get; private set; } = null!;
    public Usuario Usuario { get; private set; } = null!;

    private MovimentacaoEstoque() { }

    public MovimentacaoEstoque(
        Guid produtoId,
        TipoMovimentacao tipo,
        int quantidade,
        Guid usuarioId,
        string? observacao = null)
    {
        ValidarQuantidade(quantidade);

        ProdutoId = produtoId;
        Tipo = tipo;
        Quantidade = quantidade;
        UsuarioId = usuarioId;
        Observacao = observacao;
    }

    public void AtualizarObservacao(string observacao)
    {
        Observacao = observacao;
        AtualizarDataModificacao();
    }

    private static void ValidarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero", nameof(quantidade));
    }
}
