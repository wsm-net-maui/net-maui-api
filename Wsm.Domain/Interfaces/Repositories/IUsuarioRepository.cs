using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<bool> EmailJaExisteAsync(string email, Guid? usuarioIdExcluir = null);
}
