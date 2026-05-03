using Wsm.Aplication.DTOs.FuncionarioPerfis;
using Wsm.Aplication.Interfaces;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class FuncionarioPerfilService : IFuncionarioPerfilService
{
    private readonly IFuncionarioPerfilRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public FuncionarioPerfilService(IFuncionarioPerfilRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<FuncionarioPerfilResponseDto>> ObterTodosAsync()
    {
        var perfis = await _repository.ObterTodosAsync();
        return perfis.Select(ToResponseDto);
    }

    public async Task<FuncionarioPerfilResponseDto?> ObterPorIdAsync(Guid id)
    {
        var perfil = await _repository.ObterPorIdAsync(id);
        return perfil == null ? null : ToResponseDto(perfil);
    }

    public async Task<FuncionarioPerfilResponseDto?> ObterPorUsuarioIdAsync(Guid usuarioId)
    {
        var perfil = await _repository.ObterPorUsuarioIdAsync(usuarioId);
        return perfil == null ? null : ToResponseDto(perfil);
    }

    public async Task<FuncionarioPerfilResponseDto> CriarAsync(FuncionarioPerfilCreateDto dto)
    {
        var perfil = new FuncionarioPerfil(dto.UsuarioId, dto.Especialidade, dto.Descricao);
        await _repository.AdicionarAsync(perfil);
        await _unitOfWork.CommitAsync();
        return ToResponseDto(perfil);
    }

    public async Task AtualizarAsync(Guid id, FuncionarioPerfilUpdateDto dto)
    {
        var perfil = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"FuncionarioPerfil {id} nao encontrado.");
        perfil.Atualizar(dto.Especialidade, dto.Descricao);
        _repository.Atualizar(perfil);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var perfil = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"FuncionarioPerfil {id} nao encontrado.");
        _repository.Remover(perfil);
        await _unitOfWork.CommitAsync();
    }

    private static FuncionarioPerfilResponseDto ToResponseDto(FuncionarioPerfil p) => new()
    {
        Id = p.Id,
        UsuarioId = p.UsuarioId,
        Especialidade = p.Especialidade,
        Descricao = p.Descricao,
        Ativo = p.Ativo
    };
}
