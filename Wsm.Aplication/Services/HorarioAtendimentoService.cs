using Wsm.Aplication.DTOs.HorariosAtendimento;
using Wsm.Aplication.Interfaces;
using Wsm.Domain.Entities;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class HorarioAtendimentoService : IHorarioAtendimentoService
{
    private readonly IHorarioAtendimentoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public HorarioAtendimentoService(IHorarioAtendimentoRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<HorarioAtendimentoResponseDto>> ObterTodosPorFuncionarioAsync(Guid funcionarioPerfilId)
    {
        var horarios = await _repository.ObterPorFuncionarioPerfilIdAsync(funcionarioPerfilId);
        return horarios.Select(ToResponseDto);
    }

    public async Task<HorarioAtendimentoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var horario = await _repository.ObterPorIdAsync(id);
        return horario == null ? null : ToResponseDto(horario);
    }

    public async Task<HorarioAtendimentoResponseDto> CriarAsync(HorarioAtendimentoCreateDto dto)
    {
        var horario = new HorarioAtendimento(dto.FuncionarioPerfilId, dto.DiaSemana, dto.HoraInicio, dto.HoraFim, dto.IntervaloMinutos);
        await _repository.AdicionarAsync(horario);
        await _unitOfWork.CommitAsync();
        return ToResponseDto(horario);
    }

    public async Task AtualizarAsync(Guid id, HorarioAtendimentoUpdateDto dto)
    {
        var horario = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"HorarioAtendimento {id} nao encontrado.");
        horario.Atualizar(dto.DiaSemana, dto.HoraInicio, dto.HoraFim, dto.IntervaloMinutos);
        _repository.Atualizar(horario);
        await _unitOfWork.CommitAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var horario = await _repository.ObterPorIdAsync(id) ?? throw new KeyNotFoundException($"HorarioAtendimento {id} nao encontrado.");
        _repository.Remover(horario);
        await _unitOfWork.CommitAsync();
    }

    private static HorarioAtendimentoResponseDto ToResponseDto(HorarioAtendimento h) => new()
    {
        Id = h.Id,
        FuncionarioPerfilId = h.FuncionarioPerfilId,
        DiaSemana = h.DiaSemana,
        HoraInicio = h.HoraInicio,
        HoraFim = h.HoraFim,
        IntervaloMinutos = h.IntervaloMinutos,
        Ativo = h.Ativo
    };
}
