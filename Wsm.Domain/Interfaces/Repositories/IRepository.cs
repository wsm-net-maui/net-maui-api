using System.Linq.Expressions;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ObterTodosAsync();
    Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicado);
    Task<TEntity?> ObterPrimeiroAsync(Expression<Func<TEntity, bool>> predicado);
    Task AdicionarAsync(TEntity entidade);
    void Atualizar(TEntity entidade);
    void Remover(TEntity entidade);
    Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicado);
    Task<int> ContarAsync(Expression<Func<TEntity, bool>> predicado);
}
