using Wsm.Domain.Entities;
using Wsm.Domain.Enums;

namespace Wsm.Tests;

public class DomainRulesTests
{
    [Fact]
    public void Pedido_Deve_Recalcular_DescontoPercentual_Ao_Adicionar_Itens()
    {
        var pedido = new Pedido(Guid.NewGuid());
        var voucher = new Voucher(
            codigo: "DESC10",
            tipoDesconto: TipoDescontoVoucher.Percentual,
            valorDesconto: 10,
            dataInicio: DateTime.UtcNow.AddDays(-1),
            dataFim: DateTime.UtcNow.AddDays(1));

        pedido.AdicionarItem(Guid.NewGuid(), "Produto A", 1, 100m);
        pedido.AplicarVoucher(voucher);
        pedido.AdicionarItem(Guid.NewGuid(), "Produto B", 1, 100m);

        Assert.Equal(200m, pedido.ValorBruto);
        Assert.Equal(20m, pedido.ValorDesconto);
        Assert.Equal(180m, pedido.ValorTotal);
    }

    [Fact]
    public void Pedido_Deve_Remover_Voucher_Quando_Ele_Se_Tornar_Indisponivel()
    {
        var pedido = new Pedido(Guid.NewGuid());
        var voucher = new Voucher(
            codigo: "USO1",
            tipoDesconto: TipoDescontoVoucher.ValorFixo,
            valorDesconto: 20,
            dataInicio: DateTime.UtcNow.AddDays(-1),
            dataFim: DateTime.UtcNow.AddDays(1),
            usoMaximo: 1);

        var produtoId = Guid.NewGuid();
        pedido.AdicionarItem(produtoId, "Produto A", 2, 100m);
        pedido.AplicarVoucher(voucher);

        voucher.RegistrarUso();
        pedido.AdicionarItem(produtoId, "Produto A", 1, 100m);

        Assert.Null(pedido.Voucher);
        Assert.Null(pedido.VoucherId);
        Assert.Equal(300m, pedido.ValorBruto);
        Assert.Equal(0m, pedido.ValorDesconto);
        Assert.Equal(300m, pedido.ValorTotal);
    }

    [Fact]
    public void Carrinho_Deve_Recalcular_DescontoPercentual_Ao_Atualizar_Quantidade()
    {
        var carrinho = new Carrinho(Guid.NewGuid());
        var produtoId = Guid.NewGuid();
        var voucher = new Voucher(
            codigo: "CARR10",
            tipoDesconto: TipoDescontoVoucher.Percentual,
            valorDesconto: 10,
            dataInicio: DateTime.UtcNow.AddDays(-1),
            dataFim: DateTime.UtcNow.AddDays(1));

        carrinho.AdicionarItem(produtoId, "Produto C", 1, 50m);
        carrinho.AplicarVoucher(voucher);
        carrinho.AtualizarQuantidadeItem(produtoId, 3);

        Assert.Equal(150m, carrinho.ValorBruto);
        Assert.Equal(15m, carrinho.ValorDesconto);
        Assert.Equal(135m, carrinho.ValorTotal);
    }

    [Fact]
    public void Voucher_PodeSerAplicado_Deve_Retornar_Falso_Quando_Expirado()
    {
        var voucher = new Voucher(
            codigo: "EXPIRA",
            tipoDesconto: TipoDescontoVoucher.ValorFixo,
            valorDesconto: 10,
            dataInicio: DateTime.UtcNow.AddDays(-10),
            dataFim: DateTime.UtcNow.AddDays(-1));

        var podeAplicar = voucher.PodeSerAplicado(100m, DateTime.UtcNow);

        Assert.False(podeAplicar);
    }
}
