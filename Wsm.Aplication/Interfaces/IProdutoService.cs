using Wsm.Aplication.DTOs.Produtos;

namespace Wsm.Aplication.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> ObterTodosAsync();
    Task<ProdutoResponseDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ProdutoResponseDto>> ObterPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<ProdutoResponseDto>> ObterAbaixoEstoqueMinimoAsync();
    Task<ProdutoResponseDto> CriarAsync(CriarProdutoRequestDto request);
    Task<ProdutoResponseDto> AtualizarAsync(Guid id, AtualizarProdutoRequestDto request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
