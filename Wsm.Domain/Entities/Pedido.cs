using Wsm.Domain.Enums;

namespace Wsm.Domain.Entities;

public class Pedido : BaseEntity
{
    public string Numero { get; private set; }
    public Guid UsuarioId { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal ValorTotal { get; private set; }

    public Usuario Usuario { get; private set; } = null!;

    private readonly List<PedidoItem> _itens = new();
    public IReadOnlyCollection<PedidoItem> Itens => _itens.AsReadOnly();

    private Pedido() { }

    public Pedido(Guid usuarioId)
    {
        UsuarioId = usuarioId;
        Numero = GerarNumero();
        Status = StatusPedido.Pendente;
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
        ValorTotal = _itens.Sum(i => i.CalcularSubtotal());
    }

    private static string GerarNumero()
    {
        return $"PED-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}
