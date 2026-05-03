using Wsm.Aplication.DTOs.Servicos;
using Wsm.Aplication.Interfaces;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class ServicoService : IServicoService
{
    private readonly IServicoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ServicoService(IServicoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ServicoResponseDto>> ObterTodosAsync()
    {
        var servicos = await _repository.ObterTodosAsync();
        return servicos.Select(ToResponseDto);
    }

    public async Task<ServicoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var servico = await _repository.ObterPorIdAsync(id);
        return servico == null ? null : ToResponseDto(servico);
    }

    public async Task<ServicoResponseDto> CriarAsync(ServicoCreateDto dto)
    {
        var servico = new Servico(dto.Nome, dto.Preco, dto.DuracaoMinutos, dto.Descricao);
        await _repository.AdicionarAsync(servico);
        await _unitOfWork.CommitAsync();
        return ToResponseDto(servico);
    }

    public async Task AtualizarAsync(Guid id, ServicoUpdateDto dto)
    {
        var servico = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Servico {id} nao encontrado.");
        servico.Atualizar(dto.Nome, dto.Preco, dto.DuracaoMinutos, dto.Descricao);
        _repository.Atualizar(servico);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var servico = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"Servico {id} nao encontrado.");
        _repository.Remover(servico);
        await _unitOfWork.CommitAsync();
    }

    private static ServicoResponseDto ToResponseDto(Servico s) => new()
    {
        Id = s.Id,
        Nome = s.Nome,
        Descricao = s.Descricao,
        Preco = s.Preco,
        DuracaoMinutos = s.DuracaoMinutos,
        Ativo = s.Ativo
    };
}
