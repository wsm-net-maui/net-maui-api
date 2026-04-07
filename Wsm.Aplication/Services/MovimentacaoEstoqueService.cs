using Wsm.Aplication.DTOs.Movimentacoes;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Enums;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class MovimentacaoEstoqueService : IMovimentacaoEstoqueService
{
    private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MovimentacaoEstoqueService(
        IMovimentacaoEstoqueRepository movimentacaoRepository,
        IProdutoRepository produtoRepository,
        IUnitOfWork unitOfWork)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MovimentacaoEstoqueResponseDto>> ObterTodasAsync()
    {
        var movimentacoes = await _movimentacaoRepository.ObterTodosAsync();
        return movimentacoes.Select(MovimentacaoEstoqueMapper.ToResponseDto);
    }

    public async Task<MovimentacaoEstoqueResponseDto?> ObterPorIdAsync(Guid id)
    {
        var movimentacao = await _movimentacaoRepository.ObterPorIdAsync(id);
        return movimentacao != null ? MovimentacaoEstoqueMapper.ToResponseDto(movimentacao) : null;
    }

    public async Task<IEnumerable<MovimentacaoEstoqueResponseDto>> ObterPorProdutoAsync(Guid produtoId)
    {
        var movimentacoes = await _movimentacaoRepository.ObterPorProdutoAsync(produtoId);
        return movimentacoes.Select(MovimentacaoEstoqueMapper.ToResponseDto);
    }

    public async Task<MovimentacaoEstoqueResponseDto> CriarAsync(CriarMovimentacaoRequestDto request, Guid usuarioId)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(request.ProdutoId)
            ?? throw new KeyNotFoundException("Produto não encontrado");

        var movimentacao = MovimentacaoEstoqueMapper.ToEntity(request, usuarioId);

        if (request.Tipo == TipoMovimentacao.Entrada || request.Tipo == TipoMovimentacao.Devolucao)
        {
            produto.AdicionarEstoque(request.Quantidade);
        }
        else if (request.Tipo == TipoMovimentacao.Saida)
        {
            produto.RemoverEstoque(request.Quantidade);
        }

        await _movimentacaoRepository.AdicionarAsync(movimentacao);
        _produtoRepository.Atualizar(produto);
        await _unitOfWork.CommitAsync();

        var movimentacaoCompleta = await _movimentacaoRepository.ObterPorIdAsync(movimentacao.Id);
        return MovimentacaoEstoqueMapper.ToResponseDto(movimentacaoCompleta!);
    }
}
