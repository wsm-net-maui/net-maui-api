using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IClientePerfilRepository : IRepository<ClientePerfil>
{
    Task<ClientePerfil?> ObterPorUsuarioIdAsync(Guid usuarioId);
}
