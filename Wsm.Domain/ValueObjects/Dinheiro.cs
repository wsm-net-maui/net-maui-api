namespace Wsm.Domain.ValueObjects;

public record Dinheiro
{
    public decimal Valor { get; }

    public Dinheiro(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));

        Valor = Math.Round(valor, 2);
    }

    public static Dinheiro operator +(Dinheiro a, Dinheiro b) => new(a.Valor + b.Valor);
    public static Dinheiro operator -(Dinheiro a, Dinheiro b) => new(a.Valor - b.Valor);
    public static Dinheiro operator *(Dinheiro a, decimal multiplicador) => new(a.Valor * multiplicador);
    public static bool operator >(Dinheiro a, Dinheiro b) => a.Valor > b.Valor;
    public static bool operator <(Dinheiro a, Dinheiro b) => a.Valor < b.Valor;
    public static bool operator >=(Dinheiro a, Dinheiro b) => a.Valor >= b.Valor;
    public static bool operator <=(Dinheiro a, Dinheiro b) => a.Valor <= b.Valor;

    public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;
    public static explicit operator Dinheiro(decimal valor) => new(valor);

    public override string ToString() => $"R$ {Valor:N2}";
}
