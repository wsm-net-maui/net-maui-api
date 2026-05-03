using Wsm.Aplication.DTOs.ClientePerfis;
using Wsm.Aplication.Interfaces;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class ClientePerfilService : IClientePerfilService
{
    private readonly IClientePerfilRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientePerfilService(IClientePerfilRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClientePerfilResponseDto>> ObterTodosAsync()
    {
        var perfis = await _repository.ObterTodosAsync();
        return perfis.Select(ToResponseDto);
    }

    public async Task<ClientePerfilResponseDto?> ObterPorIdAsync(Guid id)
    {
        var perfil = await _repository.ObterPorIdAsync(id);
        return perfil == null ? null : ToResponseDto(perfil);
    }

    public async Task<ClientePerfilResponseDto?> ObterPorUsuarioIdAsync(Guid usuarioId)
    {
        var perfil = await _repository.ObterPorUsuarioIdAsync(usuarioId);
        return perfil == null ? null : ToResponseDto(perfil);
    }

    public async Task<ClientePerfilResponseDto> CriarAsync(ClientePerfilCreateDto dto)
    {
        var perfil = new ClientePerfil(dto.UsuarioId, dto.Telefone, dto.Observacoes);
        await _repository.AdicionarAsync(perfil);
        await _unitOfWork.CommitAsync();
        return ToResponseDto(perfil);
    }

    public async Task AtualizarAsync(Guid id, ClientePerfilUpdateDto dto)
    {
        var perfil = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"ClientePerfil {id} nao encontrado.");
        perfil.Atualizar(dto.Telefone, dto.Observacoes);
        _repository.Atualizar(perfil);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var perfil = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"ClientePerfil {id} nao encontrado.");
        _repository.Remover(perfil);
        await _unitOfWork.CommitAsync();
    }

    private static ClientePerfilResponseDto ToResponseDto(ClientePerfil p) => new()
    {
        Id = p.Id,
        UsuarioId = p.UsuarioId,
        Telefone = p.Telefone,
        Observacoes = p.Observacoes,
        Ativo = p.Ativo
    };
}
