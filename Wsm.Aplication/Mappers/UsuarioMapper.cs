using Wsm.Aplication.DTOs.Auth;
using Wsm.Domain.Entities;

namespace Wsm.Aplication.Mappers;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cargo = usuario.Cargo
        };
    }
}
