namespace Wsm.Aplication.DTOs.HorariosAtendimento;

public class HorarioAtendimentoResponseDto
{
    public Guid Id { get; set; }
    public Guid FuncionarioPerfilId { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim { get; set; }
    public int IntervaloMinutos { get; set; }
    public bool Ativo { get; set; }
}
