using Wsm.Aplication.DTOs.HorariosAtendimento;

namespace Wsm.Aplication.Interfaces;

public interface IHorarioAtendimentoService
{
    Task<IEnumerable<HorarioAtendimentoResponseDto>> ObterTodosPorFuncionarioAsync(Guid funcionarioPerfilId);
    Task<HorarioAtendimentoResponseDto?> ObterPorIdAsync(Guid id);
    Task<HorarioAtendimentoResponseDto> CriarAsync(HorarioAtendimentoCreateDto dto);
    Task AtualizarAsync(Guid id, HorarioAtendimentoUpdateDto dto);
    Task RemoverAsync(Guid id);
}
