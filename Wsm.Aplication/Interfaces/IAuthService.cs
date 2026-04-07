using Wsm.Aplication.DTOs.Auth;

namespace Wsm.Aplication.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<UsuarioDto> RegistrarAsync(RegistroRequestDto request);
}
