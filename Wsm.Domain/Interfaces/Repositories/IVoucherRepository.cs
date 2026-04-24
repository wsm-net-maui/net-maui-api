using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface IVoucherRepository : IRepository<Voucher>
{
    Task<Voucher?> ObterPorCodigoAsync(string codigo);
    Task<bool> CodigoJaExisteAsync(string codigo, Guid? voucherIdExcluir = null);
}
