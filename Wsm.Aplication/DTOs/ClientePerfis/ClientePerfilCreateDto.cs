namespace Wsm.Aplication.DTOs.ClientePerfis;

public class ClientePerfilCreateDto
{
    public Guid UsuarioId { get; set; }
    public string? Telefone { get; set; }
    public string? Observacoes { get; set; }
}
