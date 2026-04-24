using Wsm.Aplication.DTOs.Carrinhos;

namespace Wsm.Aplication.Interfaces;

public interface ICarrinhoService
{
    Task<CarrinhoResponseDto> ObterMeuCarrinhoAsync(Guid usuarioId);
    Task<CarrinhoResponseDto> AdicionarItemAsync(Guid usuarioId, AdicionarItemCarrinhoRequestDto request);
    Task<CarrinhoResponseDto> AtualizarQuantidadeItemAsync(Guid usuarioId, Guid produtoId, AtualizarQuantidadeCarrinhoRequestDto request);
    Task<CarrinhoResponseDto> RemoverItemAsync(Guid usuarioId, Guid produtoId);
    Task<CarrinhoResponseDto> AplicarVoucherAsync(Guid usuarioId, AplicarVoucherCarrinhoRequestDto request);
    Task<CarrinhoResponseDto> RemoverVoucherAsync(Guid usuarioId);
    Task LimparAsync(Guid usuarioId);
}
