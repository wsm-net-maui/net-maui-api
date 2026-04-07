using Microsoft.AspNetCore.Mvc;
using net_maui_api.Attributes;
using Wsm.Aplication.DTOs.Auth;
using Wsm.Aplication.Interfaces;

namespace net_maui_api.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var resultado = await _authService.LoginAsync(request);
        return Success(resultado, "Login realizado com sucesso");
    }

    [HttpPost("registro")]
    [ValidateModel]
    [LogExecutionTime]
    public async Task<IActionResult> Registro([FromBody] RegistroRequestDto request)
    {
        var usuario = await _authService.RegistrarAsync(request);
        return Created(usuario, "Usuário registrado com sucesso");
    }
}
