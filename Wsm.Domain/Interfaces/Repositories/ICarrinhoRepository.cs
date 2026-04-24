using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface ICarrinhoRepository : IRepository<Carrinho>
{
    Task<Carrinho?> ObterPorUsuarioAsync(Guid usuarioId);
}
