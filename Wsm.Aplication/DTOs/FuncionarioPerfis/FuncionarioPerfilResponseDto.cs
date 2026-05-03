namespace Wsm.Aplication.DTOs.FuncionarioPerfis;

public class FuncionarioPerfilResponseDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Especialidade { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
}
