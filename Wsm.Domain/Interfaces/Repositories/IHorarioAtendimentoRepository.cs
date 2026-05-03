using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IHorarioAtendimentoRepository : IRepository<HorarioAtendimento>
{
    Task<IEnumerable<HorarioAtendimento>> ObterPorFuncionarioPerfilIdAsync(Guid funcionarioPerfilId);
}
