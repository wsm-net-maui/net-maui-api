using Wsm.Aplication.DTOs.Servicos;

namespace Wsm.Aplication.Interfaces;

public interface IServicoService
{
    Task<IEnumerable<ServicoResponseDto>> ObterTodosAsync();
    Task<ServicoResponseDto?> ObterPorIdAsync(Guid id);
    Task<ServicoResponseDto> CriarAsync(ServicoCreateDto dto);
    Task AtualizarAsync(Guid id, ServicoUpdateDto dto);
    Task RemoverAsync(Guid id);
}
