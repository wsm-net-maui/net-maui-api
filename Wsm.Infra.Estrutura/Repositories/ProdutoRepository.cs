using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Produto?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterPorCategoriaAsync(Guid categoriaId)
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterAbaixoEstoqueMinimoAsync()
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .Where(p => p.QuantidadeEstoque < p.EstoqueMinimo)
            .ToListAsync();
    }

    public async Task<Produto?> ObterPorCodigoBarrasAsync(string codigoBarras)
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras);
    }

    public async Task<IEnumerable<Produto>> ObterComMovimentacoesAsync()
    {
        return await _dbSet
            .Include(p => p.Categoria)
            .Include(p => p.Movimentacoes)
            .ToListAsync();
    }
}
