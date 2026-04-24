using Wsm.Domain.Enums;

namespace Wsm.Domain.Entities;

public class Voucher : BaseEntity
{
    public string Codigo { get; private set; }
    public TipoDescontoVoucher TipoDesconto { get; private set; }
    public decimal ValorDesconto { get; private set; }
    public decimal? ValorMinimoPedido { get; private set; }
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public int? UsoMaximo { get; private set; }
    public int UsoAtual { get; private set; }

    private Voucher() { }

    public Voucher(
        string codigo,
        TipoDescontoVoucher tipoDesconto,
        decimal valorDesconto,
        DateTime dataInicio,
        DateTime dataFim,
        decimal? valorMinimoPedido = null,
        int? usoMaximo = null)
    {
        ValidarCodigo(codigo);
        ValidarDatas(dataInicio, dataFim);
        ValidarValorDesconto(tipoDesconto, valorDesconto);
        ValidarValorMinimo(valorMinimoPedido);
        ValidarUsoMaximo(usoMaximo);

        Codigo = codigo.Trim().ToUpperInvariant();
        TipoDesconto = tipoDesconto;
        ValorDesconto = valorDesconto;
        DataInicio = dataInicio;
        DataFim = dataFim;
        ValorMinimoPedido = valorMinimoPedido;
        UsoMaximo = usoMaximo;
        UsoAtual = 0;
    }

    public bool PodeSerAplicado(decimal valorPedido, DateTime dataReferencia)
    {
        if (!Ativo)
            return false;

        if (dataReferencia < DataInicio || dataReferencia > DataFim)
            return false;

        if (ValorMinimoPedido.HasValue && valorPedido < ValorMinimoPedido.Value)
            return false;

        if (UsoMaximo.HasValue && UsoAtual >= UsoMaximo.Value)
            return false;

        return true;
    }

    public decimal CalcularDesconto(decimal valorPedido)
    {
        if (valorPedido <= 0)
            return 0;

        var desconto = TipoDesconto switch
        {
            TipoDescontoVoucher.ValorFixo => ValorDesconto,
            TipoDescontoVoucher.Percentual => valorPedido * (ValorDesconto / 100),
            _ => 0
        };

        if (desconto < 0)
            desconto = 0;

        return desconto > valorPedido ? valorPedido : desconto;
    }

    public void RegistrarUso()
    {
        if (UsoMaximo.HasValue && UsoAtual >= UsoMaximo.Value)
            throw new InvalidOperationException("Voucher atingiu o limite de uso");

        UsoAtual++;
        AtualizarDataModificacao();
    }

    private static void ValidarCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("Código do voucher é obrigatório", nameof(codigo));

        if (codigo.Length < 4)
            throw new ArgumentException("Código do voucher deve ter no mínimo 4 caracteres", nameof(codigo));

        if (codigo.Length > 50)
            throw new ArgumentException("Código do voucher deve ter no máximo 50 caracteres", nameof(codigo));
    }

    private static void ValidarDatas(DateTime dataInicio, DateTime dataFim)
    {
        if (dataFim < dataInicio)
            throw new ArgumentException("Data fim deve ser maior ou igual à data início");
    }

    private static void ValidarValorDesconto(TipoDescontoVoucher tipoDesconto, decimal valorDesconto)
    {
        if (valorDesconto <= 0)
            throw new ArgumentException("Valor do desconto deve ser maior que zero", nameof(valorDesconto));

        if (tipoDesconto == TipoDescontoVoucher.Percentual && valorDesconto > 100)
            throw new ArgumentException("Desconto percentual não pode ser maior que 100", nameof(valorDesconto));
    }

    private static void ValidarValorMinimo(decimal? valorMinimoPedido)
    {
        if (valorMinimoPedido.HasValue && valorMinimoPedido.Value < 0)
            throw new ArgumentException("Valor mínimo do pedido não pode ser negativo", nameof(valorMinimoPedido));
    }

    private static void ValidarUsoMaximo(int? usoMaximo)
    {
        if (usoMaximo.HasValue && usoMaximo.Value <= 0)
            throw new ArgumentException("Uso máximo deve ser maior que zero", nameof(usoMaximo));
    }
}
