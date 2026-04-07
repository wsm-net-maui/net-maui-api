using Wsm.Domain.Entities;

namespace Wsm.Infra.Core.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
