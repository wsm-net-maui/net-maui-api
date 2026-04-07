using Wsm.Aplication.DTOs.Categorias;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class CategoriaMapper
{
    public static CategoriaResponseDto ToResponseDto(Categoria categoria)
    {
        return new CategoriaResponseDto
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            QuantidadeProdutos = categoria.Produtos?.Count ?? 0,
            Ativo = categoria.Ativo,
            CriadoEm = categoria.CriadoEm,
            AtualizadoEm = categoria.AtualizadoEm
        };
    }

    public static Categoria ToEntity(CriarCategoriaRequestDto dto)
    {
        return new Categoria(
            nome: dto.Nome,
            descricao: dto.Descricao
        );
    }
}
