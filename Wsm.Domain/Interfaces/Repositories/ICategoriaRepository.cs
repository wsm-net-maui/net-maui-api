using Wsm.Domain.Entities;

namespace Wsm.Domain.Interfaces.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<Categoria?> ObterPorNomeAsync(string nome);
    Task<bool> NomeJaExisteAsync(string nome, Guid? categoriaIdExcluir = null);
    Task<IEnumerable<Categoria>> ObterComProdutosAsync();
}
