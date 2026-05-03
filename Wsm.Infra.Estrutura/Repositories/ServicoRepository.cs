using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class ServicoRepository : Repository<Servico>, IServicoRepository
{
    public ServicoRepository(ApplicationDbContext context) : base(context) { }
}
