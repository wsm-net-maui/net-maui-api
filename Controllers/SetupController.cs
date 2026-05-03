using Microsoft.AspNetCore.Mvc;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace net_maui_api.Controllers;

/// <summary>
/// Endpoint temporário de setup — remover após configurar os usuários.
/// </summary>
public class SetupController : BaseController
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public SetupController(
        IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    /// <summary>
    /// Atualiza o cargo de um usuário pelo e-mail. Requer a setup-key configurada.
    /// </summary>
    [HttpPatch("usuario-cargo")]
    public async Task<IActionResult> AtualizarCargo(
        [FromQuery] string email,
        [FromQuery] string cargo,
        [FromQuery] string setupKey)
    {
        var chaveEsperada = _configuration["SetupKey"] ?? "wsm-setup-2026";
        if (setupKey != chaveEsperada)
            return BadRequest("Acesso negado", "Setup key inválida.");

        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        if (usuario == null)
            return NotFound($"Usuário '{email}' não encontrado.");

        var cargoNormalizado = cargo.ToLowerInvariant() switch
        {
            "admin" or "adm" or "administrador" => "Admin",
            "user" or "usuario" or "usuário" => "User",
            _ => cargo
        };

        usuario.AtualizarDados(usuario.Nome, cargoNormalizado);
        _usuarioRepository.Atualizar(usuario);
        await _unitOfWork.CommitAsync();

        return Success(new { email = usuario.Email, cargoAtualizado = cargoNormalizado }, "Cargo atualizado com sucesso.");
    }
}

