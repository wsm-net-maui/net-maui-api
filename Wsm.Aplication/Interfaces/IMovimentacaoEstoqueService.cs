using Wsm.Aplication.DTOs.Movimentacoes;

namespace Wsm.Aplication.Interfaces;

public interface IMovimentacaoEstoqueService
{
    Task<IEnumerable<MovimentacaoEstoqueResponseDto>> ObterTodasAsync();
    Task<MovimentacaoEstoqueResponseDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<MovimentacaoEstoqueResponseDto>> ObterPorProdutoAsync(Guid produtoId);
    Task<MovimentacaoEstoqueResponseDto> CriarAsync(CriarMovimentacaoRequestDto request, Guid usuarioId);
}
