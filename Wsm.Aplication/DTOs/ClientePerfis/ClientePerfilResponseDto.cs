namespace Wsm.Aplication.DTOs.ClientePerfis;

public class ClientePerfilResponseDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string? Telefone { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
}
