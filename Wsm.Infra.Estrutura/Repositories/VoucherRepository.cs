using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Estrutura.Data;

namespace Wsm.Infra.Estrutura.Repositories;

public class VoucherRepository : Repository<Voucher>, IVoucherRepository
{
    public VoucherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Voucher?> ObterPorCodigoAsync(string codigo)
    {
        var codigoNormalizado = codigo.Trim().ToUpperInvariant();
        return await _dbSet.FirstOrDefaultAsync(v => v.Codigo == codigoNormalizado);
    }

    public async Task<bool> CodigoJaExisteAsync(string codigo, Guid? voucherIdExcluir = null)
    {
        var codigoNormalizado = codigo.Trim().ToUpperInvariant();

        var query = _dbSet.Where(v => v.Codigo == codigoNormalizado);

        if (voucherIdExcluir.HasValue)
            query = query.Where(v => v.Id != voucherIdExcluir.Value);

        return await query.AnyAsync();
    }
}
