using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido?> ObterPorNumeroAsync(string numero);
    Task<IEnumerable<Pedido>> ObterPorUsuarioAsync(Guid usuarioId);
}
