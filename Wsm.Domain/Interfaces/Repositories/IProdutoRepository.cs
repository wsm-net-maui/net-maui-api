using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> ObterPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<Produto>> ObterAbaixoEstoqueMinimoAsync();
    Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras);
    Task<IEnumerable<Produto>> ObterComMovimentacoesAsync();
}
