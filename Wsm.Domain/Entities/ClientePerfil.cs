namespace Wsm.Domain.Entities;

public class ClientePerfil : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public string? Telefone { get; private set; }
    public string? Observacoes { get; private set; }

    public Usuario? Usuario { get; private set; }

    private ClientePerfil() { }

    public ClientePerfil(Guid usuarioId, string? telefone = null, string? observacoes = null)
    {
        UsuarioId = usuarioId;
        Telefone = telefone;
        Observacoes = observacoes;
    }

    public void Atualizar(string? telefone, string? observacoes)
    {
        Telefone = telefone;
        Observacoes = observacoes;
        AtualizarDataModificacao();
    }
}
