using System.Security.Cryptography;
using System.Text;
using Wsm.Aplication.DTOs.Auth;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Core.Interfaces;

namespace Wsm.Aplication.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Credenciais inválidas");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário inativo");

        var senhaHash = GerarHashSenha(request.Senha);
        if (usuario.SenhaHash != senhaHash)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        var token = _tokenService.GerarToken(usuario);
        var expiracao = DateTime.UtcNow.AddHours(8);

        return new LoginResponseDto
        {
            Token = token,
            Expiracao = expiracao,
            Usuario = UsuarioMapper.ToDto(usuario)
        };
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistroRequestDto request)
    {
        if (request.Senha != request.ConfirmacaoSenha)
            throw new ArgumentException("Senha e confirmação não conferem");

        if (await _usuarioRepository.EmailJaExisteAsync(request.Email))
            throw new ArgumentException("Email já cadastrado");

        var senhaHash = GerarHashSenha(request.Senha);
        var cargo = NormalizarCargo(request.Cargo);
        var usuario = new Usuario(request.Nome, request.Email, senhaHash, cargo);

        await _usuarioRepository.AdicionarAsync(usuario);
        await _unitOfWork.CommitAsync();

        return UsuarioMapper.ToDto(usuario);
    }

    private static string GerarHashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private static string? NormalizarCargo(string? cargo)
    {
        return cargo?.ToLowerInvariant() switch
        {
            "admin" or "adm" or "administrador" => "Admin",
            "user" or "usuario" or "usuário" => "User",
            _ => cargo
        };
    }
}
