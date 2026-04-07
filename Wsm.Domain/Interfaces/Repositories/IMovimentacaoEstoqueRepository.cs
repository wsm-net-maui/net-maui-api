using Wsm.Domain.Entities;
using Wsm.Domain.Enums;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IMovimentacaoEstoqueRepository : IRepository<MovimentacaoEstoque>
{
    Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<MovimentacaoEstoque>> ObterPorUsuarioAsync(Guid usuarioId);
    Task<IEnumerable<MovimentacaoEstoque>> ObterPorTipoAsync(TipoMovimentacao tipo);
    Task<IEnumerable<MovimentacaoEstoque>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
}
