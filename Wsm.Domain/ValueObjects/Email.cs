namespace Wsm.Domain.ValueObjects;

public record Email
{
    public string Valor { get; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Email não pode ser vazio", nameof(valor));

        if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Email inválido", nameof(valor));

        Valor = valor.ToLowerInvariant();
    }

    public static implicit operator string(Email email) => email.Valor;
    public static explicit operator Email(string valor) => new(valor);

    public override string ToString() => Valor;
}
