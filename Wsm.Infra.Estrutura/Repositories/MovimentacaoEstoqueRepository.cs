using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Enums;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class MovimentacaoEstoqueRepository : Repository<MovimentacaoEstoque>, IMovimentacaoEstoqueRepository
{
    public MovimentacaoEstoqueRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<MovimentacaoEstoque?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(m => m.Produto)
            .Include(m => m.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public override async Task<IEnumerable<MovimentacaoEstoque>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(m => m.Produto)
            .Include(m => m.Usuario)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(Guid produtoId)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .Where(m => m.ProdutoId == produtoId)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        return await _dbSet
            .Include(m => m.Produto)
            .Where(m => m.UsuarioId == usuarioId)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorTipoAsync(TipoMovimentacao tipo)
    {
        return await _dbSet
            .Include(m => m.Produto)
            .Include(m => m.Usuario)
            .Where(m => m.Tipo == tipo)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Include(m => m.Produto)
            .Include(m => m.Usuario)
            .Where(m => m.CriadoEm >= dataInicio && m.CriadoEm <= dataFim)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }
}
