using Wsm.Domain.Enums;

namespace Wsm.Domain.Entities;

public class Pedido : BaseEntity
{
    public string Numero { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorBruto { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public decimal ValorTotal { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Voucher? Voucher { get; private set; }

    private readonly List<PedidoItem> _itens = new();
    public IReadOnlyCollection<PedidoItem> Itens => _itens.AsReadOnly();

    private Pedido() { }

    public Pedido(Guid usuarioId)
    {
        UsuarioId = usuarioId;
        Numero = GerarNumero();
        Status = StatusPedido.Pendente;
        ValorBruto = 0;
        ValorDesconto = 0;
        ValorTotal = 0;
    }

    public void AdicionarItem(Guid produtoId, string produtoNome, int quantidade, decimal precoUnitario)
    {
        if (Status != StatusPedido.Pendente)
            throw new InvalidOperationException("Só é possível alterar itens de pedidos pendentes");

        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId && i.PrecoUnitario == precoUnitario);

        if (itemExistente is null)
        {
            _itens.Add(new PedidoItem(produtoId, produtoNome, quantidade, precoUnitario));
        }
        else
        {
            _itens.Remove(itemExistente);
            var novaQuantidade = itemExistente.Quantidade + quantidade;
            _itens.Add(new PedidoItem(produtoId, itemExistente.ProdutoNome, novaQuantidade, itemExistente.PrecoUnitario));
        }

        RecalcularTotal();
        AtualizarDataModificacao();
    }

    public void AplicarVoucher(Voucher voucher)
    {
        if (Status != StatusPedido.Pendente)
            throw new InvalidOperationException("Só é possível aplicar voucher em pedidos pendentes");

        if (_itens.Count == 0)
            throw new InvalidOperationException("Não é possível aplicar voucher em pedido sem itens");

        if (!voucher.PodeSerAplicado(ValorBruto, DateTime.UtcNow))
            throw new InvalidOperationException("Voucher inválido para este pedido");

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
        if (Status != StatusPedido.Pendente)
            throw new InvalidOperationException("Só é possível remover voucher de pedidos pendentes");

        VoucherId = null;
        Voucher = null;
        ValorDesconto = 0;
        ValorTotal = ValorBruto;
        AtualizarDataModificacao();
    }

    public void Concluir()
    {
        if (Status != StatusPedido.Pendente)
            throw new InvalidOperationException("Apenas pedidos pendentes podem ser concluídos");

        if (_itens.Count == 0)
            throw new InvalidOperationException("Não é possível concluir pedido sem itens");

        Status = StatusPedido.Concluido;
        AtualizarDataModificacao();
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Pedido já está cancelado");

        if (Status == StatusPedido.Concluido)
            throw new InvalidOperationException("Pedido concluído não pode ser cancelado");

        Status = StatusPedido.Cancelado;
        AtualizarDataModificacao();
    }

    private void RecalcularTotal()
    {
        ValorBruto = _itens.Sum(i => i.CalcularSubtotal());

        if (Voucher is not null)
        {
            if (Voucher.PodeSerAplicado(ValorBruto, DateTime.UtcNow))
            {
                ValorDesconto = Voucher.CalcularDesconto(ValorBruto);
            }
            else
            {
                VoucherId = null;
                Voucher = null;
                ValorDesconto = 0;
            }
        }
        else
        {
            ValorDesconto = 0;
        }

        if (ValorDesconto > ValorBruto)
            ValorDesconto = ValorBruto;

        ValorTotal = ValorBruto - ValorDesconto;

        if (ValorTotal < 0)
            ValorTotal = 0;
    }

    private static string GerarNumero()
    {
        return $"PED-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
