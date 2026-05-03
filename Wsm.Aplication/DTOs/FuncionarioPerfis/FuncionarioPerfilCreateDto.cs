namespace Wsm.Aplication.DTOs.FuncionarioPerfis;

public class FuncionarioPerfilCreateDto
{
    public Guid UsuarioId { get; set; }
    public string Especialidade { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
