using Wsm.Aplication.DTOs.Categorias;

namespace Wsm.Aplication.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> ObterTodasAsync();
    Task<CategoriaResponseDto?> ObterPorIdAsync(Guid id);
    Task<CategoriaResponseDto> CriarAsync(CriarCategoriaRequestDto request);
    Task<CategoriaResponseDto> AtualizarAsync(Guid id, AtualizarCategoriaRequestDto request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
