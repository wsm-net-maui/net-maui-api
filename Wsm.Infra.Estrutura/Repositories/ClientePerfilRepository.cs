using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class ClientePerfilRepository : Repository<ClientePerfil>, IClientePerfilRepository
{
    public ClientePerfilRepository(ApplicationDbContext context) : base(context) { }

    public async Task<ClientePerfil?> ObterPorUsuarioIdAsync(Guid usuarioId)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
    }
}
