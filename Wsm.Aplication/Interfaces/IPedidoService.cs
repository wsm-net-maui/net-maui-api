using Wsm.Aplication.DTOs.Pedidos;

namespace Wsm.Aplication.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoResponseDto>> ObterTodosAsync();
    Task<PedidoResponseDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<PedidoResponseDto>> ObterPorUsuarioAsync(Guid usuarioId);
    Task<PedidoResponseDto> CriarAsync(CriarPedidoRequestDto request, Guid usuarioId);
    Task<PedidoResponseDto> ConcluirAsync(Guid id, Guid usuarioId);
    Task CancelarAsync(Guid id);
}
