using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class HorarioAtendimentoRepository : Repository<HorarioAtendimento>, IHorarioAtendimentoRepository
{
    public HorarioAtendimentoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<HorarioAtendimento>> ObterPorFuncionarioPerfilIdAsync(Guid funcionarioPerfilId)
    {
        return await _dbSet.Where(h => h.FuncionarioPerfilId == funcionarioPerfilId).ToListAsync();
    }
}
