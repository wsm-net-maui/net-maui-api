using Wsm.Domain.Enums;

namespace Wsm.Domain.Entities;

public class Carrinho : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public decimal ValorBruto { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public decimal ValorTotal { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Voucher? Voucher { get; private set; }

    private readonly List<CarrinhoItem> _itens = new();
    public IReadOnlyCollection<CarrinhoItem> Itens => _itens.AsReadOnly();

    private Carrinho() { }

    public Carrinho(Guid usuarioId)
    {
        UsuarioId = usuarioId;
        ValorBruto = 0;
        ValorDesconto = 0;
        ValorTotal = 0;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId && i.PrecoUnitario == precoUnitario);

        if (itemExistente is null)
        {
            _itens.Add(new CarrinhoItem(produtoId, produtoNome, quantidade, precoUnitario));
        }
        else
        {
            var novaQuantidade = itemExistente.Quantidade + quantidade;
            itemExistente.AtualizarQuantidade(novaQuantidade);
        }

        RecalcularTotais();
        AtualizarDataModificacao();
    }

    public void AtualizarQuantidadeItem(Guid produtoId, int quantidade)
    {
        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId)
            ?? throw new KeyNotFoundException("Item não encontrado no carrinho");

        item.AtualizarQuantidade(quantidade);
        RecalcularTotais();
        AtualizarDataModificacao();
    }

    public void RemoverItem(Guid produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId)
            ?? throw new KeyNotFoundException("Item não encontrado no carrinho");

        _itens.Remove(item);
        RecalcularTotais();
        AtualizarDataModificacao();
    }

    public void Limpar()
    {
        _itens.Clear();
        VoucherId = null;
        Voucher = null;
        ValorBruto = 0;
        ValorDesconto = 0;
        ValorTotal = 0;
        AtualizarDataModificacao();
    }

    public void AplicarVoucher(Voucher voucher)
    {
        if (_itens.Count == 0)
            throw new InvalidOperationException("Não é possível aplicar voucher em carrinho sem itens");

        if (!voucher.PodeSerAplicado(ValorBruto, DateTime.UtcNow))
            throw new InvalidOperationException("Voucher inválido para este carrinho");

        VoucherId = voucher.Id;
        Voucher = voucher;
        ValorDesconto = voucher.CalcularDesconto(ValorBruto);
        ValorTotal = ValorBruto - ValorDesconto;

        if (ValorTotal < 0)
            ValorTotal = 0;

        AtualizarDataModificacao();
    }

    public void RemoverVoucher()
    {
        VoucherId = null;
        Voucher = null;
        ValorDesconto = 0;
        ValorTotal = ValorBruto;
        AtualizarDataModificacao();
    }

    private void RecalcularTotais()
    {
        ValorBruto = _itens.Sum(i => i.CalcularSubtotal());

        if (ValorBruto == 0)
        {
            ValorDesconto = 0;
            ValorTotal = 0;
            VoucherId = null;
            Voucher = null;
            return;
        }

        if (Voucher is not null)
        {
            if (Voucher.PodeSerAplicado(ValorBruto, DateTime.UtcNow))
            {
                ValorDesconto = Voucher.CalcularDesconto(ValorBruto);
            }
            else
            {
                ValorDesconto = 0;
                VoucherId = null;
                Voucher = null;
            }
        }
        else
        {
            ValorDesconto = 0;
        }

        if (ValorDesconto > ValorBruto)
            ValorDesconto = ValorBruto;

        ValorTotal = ValorBruto - ValorDesconto;
    }
}
