using System.Text.RegularExpressions;

namespace Wsm.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public string? Cargo { get; private set; }

    private Usuario() { }

    public Usuario(string nome, string email, string senhaHash, string? cargo = null)
    {
        ValidarNome(nome);
        ValidarEmail(email);
        ValidarSenhaHash(senhaHash);

        Nome = nome;
        Email = email.ToLowerInvariant();
        SenhaHash = senhaHash;
        Cargo = cargo;
    }

    public void AtualizarDados(string nome, string? cargo)
    {
        ValidarNome(nome);
        
        Nome = nome;
        Cargo = cargo;
        AtualizarDataModificacao();
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        ValidarSenhaHash(novaSenhaHash);
        SenhaHash = novaSenhaHash;
        AtualizarDataModificacao();
    }

    public void AlterarEmail(string novoEmail)
    {
        ValidarEmail(novoEmail);
        Email = novoEmail.ToLowerInvariant();
        AtualizarDataModificacao();
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do usuário é obrigatório", nameof(nome));

        if (nome.Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 caracteres", nameof(nome));

        if (nome.Length > 100)
            throw new ArgumentException("Nome deve ter no máximo 100 caracteres", nameof(nome));
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(email))
            throw new ArgumentException("Email inválido", nameof(email));
    }

    private static void ValidarSenhaHash(string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new ArgumentException("Hash da senha é obrigatório", nameof(senhaHash));
    }
}
