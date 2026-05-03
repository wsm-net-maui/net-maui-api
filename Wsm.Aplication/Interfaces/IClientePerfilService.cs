using Wsm.Aplication.DTOs.ClientePerfis;

namespace Wsm.Aplication.Interfaces;

public interface IClientePerfilService
{
    Task<IEnumerable<ClientePerfilResponseDto>> ObterTodosAsync();
    Task<ClientePerfilResponseDto?> ObterPorIdAsync(Guid id);
    Task<ClientePerfilResponseDto?> ObterPorUsuarioIdAsync(Guid usuarioId);
    Task<ClientePerfilResponseDto> CriarAsync(ClientePerfilCreateDto dto);
    Task AtualizarAsync(Guid id, ClientePerfilUpdateDto dto);
    Task RemoverAsync(Guid id);
}
