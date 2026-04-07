using Wsm.Aplication.DTOs.Categorias;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(
        ICategoriaRepository categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoriaResponseDto>> ObterTodasAsync()
    {
        var categorias = await _categoriaRepository.ObterComProdutosAsync();
        return categorias.Select(CategoriaMapper.ToResponseDto);
    }

    public async Task<CategoriaResponseDto?> ObterPorIdAsync(Guid id)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id);
        return categoria != null ? CategoriaMapper.ToResponseDto(categoria) : null;
    }

    public async Task<CategoriaResponseDto> CriarAsync(CriarCategoriaRequestDto request)
    {
        if (await _categoriaRepository.NomeJaExisteAsync(request.Nome))
            throw new ArgumentException("Já existe uma categoria com este nome");

        var categoria = CategoriaMapper.ToEntity(request);
        await _categoriaRepository.AdicionarAsync(categoria);
        await _unitOfWork.CommitAsync();

        return CategoriaMapper.ToResponseDto(categoria);
    }

    public async Task<CategoriaResponseDto> AtualizarAsync(Guid id, AtualizarCategoriaRequestDto request)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Categoria não encontrada");

        if (await _categoriaRepository.NomeJaExisteAsync(request.Nome, id))
            throw new ArgumentException("Já existe outra categoria com este nome");

        categoria.AtualizarDados(request.Nome, request.Descricao);
        _categoriaRepository.Atualizar(categoria);
        await _unitOfWork.CommitAsync();

        return CategoriaMapper.ToResponseDto(categoria);
    }

    public async Task AtivarAsync(Guid id)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Categoria não encontrada");

        categoria.Ativar();
        _categoriaRepository.Atualizar(categoria);
        await _unitOfWork.CommitAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Categoria não encontrada");

        categoria.Desativar();
        _categoriaRepository.Atualizar(categoria);
        await _unitOfWork.CommitAsync();
    }
}
