using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Categoria?> ObterPorNomeAsync(string nome)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Nome == nome);
    }

    public async Task<bool> NomeJaExisteAsync(string nome, Guid? categoriaIdExcluir = null)
    {
        var query = _dbSet.Where(c => c.Nome == nome);

        if (categoriaIdExcluir.HasValue)
            query = query.Where(c => c.Id != categoriaIdExcluir.Value);

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Categoria>> ObterComProdutosAsync()
    {
        return await _dbSet
            .Include(c => c.Produtos)
            .ToListAsync();
    }
}
