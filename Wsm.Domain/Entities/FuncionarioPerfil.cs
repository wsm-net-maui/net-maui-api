namespace Wsm.Domain.Entities;

public class FuncionarioPerfil : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public string Especialidade { get; private set; }
    public string? Descricao { get; private set; }

    public Usuario? Usuario { get; private set; }
    public ICollection<HorarioAtendimento> Horarios { get; private set; } = new List<HorarioAtendimento>();

    private FuncionarioPerfil() { }

    public FuncionarioPerfil(Guid usuarioId, string especialidade, string? descricao = null)
    {
        UsuarioId = usuarioId;
        Especialidade = especialidade;
        Descricao = descricao;
    }

    public void Atualizar(string especialidade, string? descricao)
    {
        Especialidade = especialidade;
        Descricao = descricao;
        AtualizarDataModificacao();
    }
}
