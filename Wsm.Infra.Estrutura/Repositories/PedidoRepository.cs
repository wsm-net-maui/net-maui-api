using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Usuario)
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public override async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return await _dbSet
            .Include(p => p.Usuario)
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync();
    }

    public async Task<Pedido?> ObterPorNumeroAsync(string numero)
    {
        return await _dbSet
            .Include(p => p.Usuario)
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Numero == numero);
    }

    public async Task<IEnumerable<Pedido>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        return await _dbSet
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync();
    }
}
