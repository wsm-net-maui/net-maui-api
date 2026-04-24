using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class CarrinhoRepository : Repository<Carrinho>, ICarrinhoRepository
{
    public CarrinhoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Carrinho?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.Voucher)
            .Include(c => c.Itens)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public override async Task<IEnumerable<Carrinho>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(c => c.Voucher)
            .Include(c => c.Itens)
            .ThenInclude(i => i.Produto)
            .OrderByDescending(c => c.CriadoEm)
            .ToListAsync();
    }

    public async Task<Carrinho?> ObterPorUsuarioAsync(Guid usuarioId)
    {
        return await _dbSet
            .Include(c => c.Voucher)
            .Include(c => c.Itens)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
    }
}
