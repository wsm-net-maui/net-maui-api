using Wsm.Aplication.DTOs.Vouchers;

namespace Wsm.Aplication.Interfaces;

public interface IVoucherService
{
    Task<IEnumerable<VoucherResponseDto>> ObterTodosAsync();
    Task<VoucherResponseDto?> ObterPorIdAsync(Guid id);
    Task<VoucherResponseDto?> ObterPorCodigoAsync(string codigo);
    Task<VoucherResponseDto> CriarAsync(CriarVoucherRequestDto request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
