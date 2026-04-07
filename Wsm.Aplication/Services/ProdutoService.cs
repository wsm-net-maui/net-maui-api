using Wsm.Aplication.DTOs.Produtos;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ObterTodosAsync()
    {
        var produtos = await _produtoRepository.ObterTodosAsync();
        return produtos.Select(ProdutoMapper.ToResponseDto);
    }

    public async Task<ProdutoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id);
        return produto != null ? ProdutoMapper.ToResponseDto(produto) : null;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ObterPorCategoriaAsync(Guid categoriaId)
    {
        var produtos = await _produtoRepository.ObterPorCategoriaAsync(categoriaId);
        return produtos.Select(ProdutoMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProdutoResponseDto>> ObterAbaixoEstoqueMinimoAsync()
    {
        var produtos = await _produtoRepository.ObterAbaixoEstoqueMinimoAsync();
        return produtos.Select(ProdutoMapper.ToResponseDto);
    }

    public async Task<ProdutoResponseDto> CriarAsync(CriarProdutoRequestDto request)
    {
        var categoriaExiste = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
        if (categoriaExiste == null)
            throw new ArgumentException("Categoria não encontrada");

        var produto = ProdutoMapper.ToEntity(request);
        await _produtoRepository.AdicionarAsync(produto);
        await _unitOfWork.CommitAsync();

        var produtoCompleto = await _produtoRepository.ObterPorIdAsync(produto.Id);
        return ProdutoMapper.ToResponseDto(produtoCompleto!);
    }

    public async Task<ProdutoResponseDto> AtualizarAsync(Guid id, AtualizarProdutoRequestDto request)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Produto não encontrado");

        var categoriaExiste = await _categoriaRepository.ObterPorIdAsync(request.CategoriaId);
        if (categoriaExiste == null)
            throw new ArgumentException("Categoria não encontrada");

        produto.AtualizarDados(
            request.Nome,
            request.PrecoCompra,
            request.PrecoVenda,
            request.EstoqueMinimo,
            request.CategoriaId,
            request.Descricao,
            request.CodigoBarras);

        _produtoRepository.Atualizar(produto);
        await _unitOfWork.CommitAsync();

        var produtoAtualizado = await _produtoRepository.ObterPorIdAsync(id);
        return ProdutoMapper.ToResponseDto(produtoAtualizado!);
    }

    public async Task AtivarAsync(Guid id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Produto não encontrado");

        produto.Ativar();
        _produtoRepository.Atualizar(produto);
        await _unitOfWork.CommitAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Produto não encontrado");

        produto.Desativar();
        _produtoRepository.Atualizar(produto);
        await _unitOfWork.CommitAsync();
    }
}
