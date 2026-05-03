using Wsm.Aplication.DTOs.FuncionarioPerfis;

namespace Wsm.Aplication.Interfaces;

public interface IFuncionarioPerfilService
{
    Task<IEnumerable<FuncionarioPerfilResponseDto>> ObterTodosAsync();
    Task<FuncionarioPerfilResponseDto?> ObterPorIdAsync(Guid id);
    Task<FuncionarioPerfilResponseDto?> ObterPorUsuarioIdAsync(Guid usuarioId);
    Task<FuncionarioPerfilResponseDto> CriarAsync(FuncionarioPerfilCreateDto dto);
    Task AtualizarAsync(Guid id, FuncionarioPerfilUpdateDto dto);
    Task RemoverAsync(Guid id);
}
