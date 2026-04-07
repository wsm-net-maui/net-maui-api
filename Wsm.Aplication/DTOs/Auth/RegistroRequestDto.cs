namespace Wsm.Aplication.DTOs.Auth;

public class RegistroRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ConfirmacaoSenha { get; set; } = string.Empty;
    public string? Cargo { get; set; }
}
