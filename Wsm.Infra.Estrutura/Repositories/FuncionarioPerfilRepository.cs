using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class FuncionarioPerfilRepository : Repository<FuncionarioPerfil>, IFuncionarioPerfilRepository
{
    public FuncionarioPerfilRepository(ApplicationDbContext context) : base(context) { }

    public async Task<FuncionarioPerfil?> ObterPorUsuarioIdAsync(Guid usuarioId)
    {
        return await _dbSet.FirstOrDefaultAsync(f => f.UsuarioId == usuarioId);
    }
}
