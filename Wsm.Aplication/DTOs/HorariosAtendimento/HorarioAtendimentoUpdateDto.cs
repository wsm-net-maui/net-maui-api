namespace Wsm.Aplication.DTOs.HorariosAtendimento;

public class HorarioAtendimentoUpdateDto
{
    public DayOfWeek DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim { get; set; }
    public int IntervaloMinutos { get; set; } = 30;
}
