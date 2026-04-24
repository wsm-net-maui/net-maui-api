using Wsm.Aplication.DTOs.Vouchers;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class VoucherService : IVoucherService
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VoucherService(IVoucherRepository voucherRepository, IUnitOfWork unitOfWork)
    {
        _voucherRepository = voucherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VoucherResponseDto>> ObterTodosAsync()
    {
        var vouchers = await _voucherRepository.ObterTodosAsync();
        return vouchers.Select(VoucherMapper.ToResponseDto);
    }

    public async Task<VoucherResponseDto?> ObterPorIdAsync(Guid id)
    {
        var voucher = await _voucherRepository.ObterPorIdAsync(id);
        return voucher is null ? null : VoucherMapper.ToResponseDto(voucher);
    }

    public async Task<VoucherResponseDto?> ObterPorCodigoAsync(string codigo)
    {
        var voucher = await _voucherRepository.ObterPorCodigoAsync(codigo);
        return voucher is null ? null : VoucherMapper.ToResponseDto(voucher);
    }

    public async Task<VoucherResponseDto> CriarAsync(CriarVoucherRequestDto request)
    {
        if (await _voucherRepository.CodigoJaExisteAsync(request.Codigo))
            throw new ArgumentException("Já existe um voucher com este código");

        var voucher = VoucherMapper.ToEntity(request);
        await _voucherRepository.AdicionarAsync(voucher);
        await _unitOfWork.CommitAsync();

        return VoucherMapper.ToResponseDto(voucher);
    }

    public async Task AtivarAsync(Guid id)
    {
        var voucher = await _voucherRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Voucher não encontrado");

        voucher.Ativar();
        _voucherRepository.Atualizar(voucher);
        await _unitOfWork.CommitAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var voucher = await _voucherRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Voucher não encontrado");

        voucher.Desativar();
        _voucherRepository.Atualizar(voucher);
        await _unitOfWork.CommitAsync();
    }
}
