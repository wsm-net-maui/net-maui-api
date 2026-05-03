using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IFuncionarioPerfilRepository : IRepository<FuncionarioPerfil>
{
    Task<FuncionarioPerfil?> ObterPorUsuarioIdAsync(Guid usuarioId);
}
