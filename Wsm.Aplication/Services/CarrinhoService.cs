using Wsm.Aplication.DTOs.Carrinhos;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class CarrinhoService : ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CarrinhoService(
        ICarrinhoRepository carrinhoRepository,
        IProdutoRepository produtoRepository,
        IVoucherRepository voucherRepository,
        IUnitOfWork unitOfWork)
    {
        _carrinhoRepository = carrinhoRepository;
        _produtoRepository = produtoRepository;
        _voucherRepository = voucherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CarrinhoResponseDto> ObterMeuCarrinhoAsync(Guid usuarioId)
    {
        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);

        if (carrinho.CriadoEm == carrinho.AtualizadoEm || carrinho.AtualizadoEm is null)
            await _unitOfWork.CommitAsync();

        return CarrinhoMapper.ToResponseDto(carrinho);
    }

    public async Task<CarrinhoResponseDto> AdicionarItemAsync(Guid usuarioId, AdicionarItemCarrinhoRequestDto request)
    {
        ValidarUsuario(usuarioId);

        var produto = await _produtoRepository.ObterPorIdAsync(request.ProdutoId)
            ?? throw new KeyNotFoundException("Produto não encontrado");

        if (!produto.Ativo)
            throw new InvalidOperationException("Produto inativo");

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
        carrinho.AdicionarItem(produto.Id, produto.Nome, request.Quantidade, produto.PrecoVenda);

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();

        var carrinhoAtualizado = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        return CarrinhoMapper.ToResponseDto(carrinhoAtualizado!);
    }

    public async Task<CarrinhoResponseDto> AtualizarQuantidadeItemAsync(Guid usuarioId, Guid produtoId, AtualizarQuantidadeCarrinhoRequestDto request)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
        carrinho.AtualizarQuantidadeItem(produtoId, request.Quantidade);

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();

        var carrinhoAtualizado = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        return CarrinhoMapper.ToResponseDto(carrinhoAtualizado!);
    }

    public async Task<CarrinhoResponseDto> RemoverItemAsync(Guid usuarioId, Guid produtoId)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
        carrinho.RemoverItem(produtoId);

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();

        var carrinhoAtualizado = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        return CarrinhoMapper.ToResponseDto(carrinhoAtualizado!);
    }

    public async Task<CarrinhoResponseDto> AplicarVoucherAsync(Guid usuarioId, AplicarVoucherCarrinhoRequestDto request)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);

        var voucher = await _voucherRepository.ObterPorCodigoAsync(request.CodigoVoucher)
            ?? throw new KeyNotFoundException("Voucher não encontrado");

        carrinho.AplicarVoucher(voucher);

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();

        var carrinhoAtualizado = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        return CarrinhoMapper.ToResponseDto(carrinhoAtualizado!);
    }

    public async Task<CarrinhoResponseDto> RemoverVoucherAsync(Guid usuarioId)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
        carrinho.RemoverVoucher();

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();

        var carrinhoAtualizado = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        return CarrinhoMapper.ToResponseDto(carrinhoAtualizado!);
    }

    public async Task LimparAsync(Guid usuarioId)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await ObterOuCriarCarrinhoAsync(usuarioId);
        carrinho.Limpar();

        _carrinhoRepository.Atualizar(carrinho);
        await _unitOfWork.CommitAsync();
    }

    private async Task<Carrinho> ObterOuCriarCarrinhoAsync(Guid usuarioId)
    {
        ValidarUsuario(usuarioId);

        var carrinho = await _carrinhoRepository.ObterPorUsuarioAsync(usuarioId);
        if (carrinho is not null)
            return carrinho;

        carrinho = new Carrinho(usuarioId);
        await _carrinhoRepository.AdicionarAsync(carrinho);
        return carrinho;
    }

    private static void ValidarUsuario(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuário não autenticado");
    }
}
