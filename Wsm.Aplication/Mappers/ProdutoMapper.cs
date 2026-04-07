using Wsm.Aplication.DTOs.Produtos;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class ProdutoMapper
{
    public static ProdutoResponseDto ToResponseDto(Produto produto)
    {
        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            CodigoBarras = produto.CodigoBarras,
            PrecoCompra = produto.PrecoCompra,
            PrecoVenda = produto.PrecoVenda,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            EstoqueMinimo = produto.EstoqueMinimo,
            AbaixoEstoqueMinimo = produto.EstaAbaixoEstoqueMinimo(),
            MargemLucro = produto.CalcularMargemLucro(),
            CategoriaId = produto.CategoriaId,
            CategoriaNome = produto.Categoria?.Nome ?? string.Empty,
            Ativo = produto.Ativo,
            CriadoEm = produto.CriadoEm,
            AtualizadoEm = produto.AtualizadoEm
        };
    }

    public static Produto ToEntity(CriarProdutoRequestDto dto)
    {
        return new Produto(
            nome: dto.Nome,
            precoCompra: dto.PrecoCompra,
            precoVenda: dto.PrecoVenda,
            estoqueMinimo: dto.EstoqueMinimo,
            categoriaId: dto.CategoriaId,
            descricao: dto.Descricao,
            codigoBarras: dto.CodigoBarras
        );
    }
}
