namespace Wsm.Domain.Entities;

public class HorarioAtendimento : BaseEntity
{
    public Guid FuncionarioPerfilId { get; private set; }
    public Guid ServicoId { get; private set; }
    public DayOfWeek DiaSemana { get; private set; }
    public TimeSpan HoraInicio { get; private set; }
    public TimeSpan HoraFim { get; private set; }
    public int IntervaloMinutos { get; private set; }

    public FuncionarioPerfil? FuncionarioPerfil { get; private set; }
    public Servico? Servico { get; private set; }

    private HorarioAtendimento() { }

    public HorarioAtendimento(Guid funcionarioPerfilId, Guid servicoId, DayOfWeek diaSemana, TimeSpan horaInicio, TimeSpan horaFim, int intervaloMinutos = 30)
    {
        FuncionarioPerfilId = funcionarioPerfilId;
        ServicoId = servicoId;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
        IntervaloMinutos = intervaloMinutos;
    }

    public void Atualizar(Guid servicoId, DayOfWeek diaSemana, TimeSpan horaInicio, TimeSpan horaFim, int intervaloMinutos)
    {
        ServicoId = servicoId;
        DiaSemana = diaSemana;
        HoraInicio = horaInicio;
        HoraFim = horaFim;
        IntervaloMinutos = intervaloMinutos;
        AtualizarDataModificacao();
    }
}
