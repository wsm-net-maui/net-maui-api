using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> predicado)
    {
        return await _dbSet.AsNoTracking().Where(predicado).ToListAsync();
    }

    public virtual async Task<TEntity?> ObterPrimeiroAsync(Expression<Func<TEntity, bool>> predicado)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicado);
    }

    public virtual async Task AdicionarAsync(TEntity entidade)
    {
        await _dbSet.AddAsync(entidade);
    }

    public virtual void Atualizar(TEntity entidade)
    {
        _dbSet.Update(entidade);
    }

    public virtual void Remover(TEntity entidade)
    {
        _dbSet.Remove(entidade);
    }

    public virtual async Task<bool> ExisteAsync(Expression<Func<TEntity, bool>> predicado)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicado);
    }

    public virtual async Task<int> ContarAsync(Expression<Func<TEntity, bool>> predicado)
    {
        return await _dbSet.AsNoTracking().CountAsync(predicado);
    }
}
