using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());
    }

    public async Task<bool> EmailJaExisteAsync(string email, Guid? usuarioIdExcluir = null)
    {
        var query = _dbSet.Where(u => u.Email == email.ToLower());

        if (usuarioIdExcluir.HasValue)
            query = query.Where(u => u.Id != usuarioIdExcluir.Value);

        return await query.AnyAsync();
    }
}
