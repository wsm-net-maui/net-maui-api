namespace Wsm.Aplication.DTOs.Servicos;

public class ServicoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int DuracaoMinutos { get; set; }
}
