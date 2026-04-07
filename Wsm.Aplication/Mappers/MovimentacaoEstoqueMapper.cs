using Wsm.Aplication.DTOs.Movimentacoes;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class MovimentacaoEstoqueMapper
{
    public static MovimentacaoEstoqueResponseDto ToResponseDto(MovimentacaoEstoque movimentacao)
    {
        return new MovimentacaoEstoqueResponseDto
        {
            Id = movimentacao.Id,
            ProdutoId = movimentacao.ProdutoId,
            ProdutoNome = movimentacao.Produto?.Nome ?? string.Empty,
            Tipo = movimentacao.Tipo,
            TipoDescricao = movimentacao.Tipo.ToString(),
            Quantidade = movimentacao.Quantidade,
            Observacao = movimentacao.Observacao,
            UsuarioId = movimentacao.UsuarioId,
            UsuarioNome = movimentacao.Usuario?.Nome ?? string.Empty,
            CriadoEm = movimentacao.CriadoEm
        };
    }

    public static MovimentacaoEstoque ToEntity(CriarMovimentacaoRequestDto dto, Guid usuarioId)
    {
        return new MovimentacaoEstoque(
            produtoId: dto.ProdutoId,
            tipo: dto.Tipo,
            quantidade: dto.Quantidade,
            usuarioId: usuarioId,
            observacao: dto.Observacao
        );
    }
}
