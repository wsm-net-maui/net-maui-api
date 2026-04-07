using Wsm.Domain.Enums;

namespace Wsm.Aplication.DTOs.Movimentacoes;

public class MovimentacaoEstoqueResponseDto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public TipoMovimentacao Tipo { get; set; }
    public string TipoDescricao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public string? Observacao { get; set; }
    public Guid UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
