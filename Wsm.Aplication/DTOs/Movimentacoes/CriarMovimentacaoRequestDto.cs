using Wsm.Domain.Enums;

namespace Wsm.Aplication.DTOs.Movimentacoes;

public class CriarMovimentacaoRequestDto
{
    public Guid ProdutoId { get; set; }
    public TipoMovimentacao Tipo { get; set; }
    public int Quantidade { get; set; }
    public string? Observacao { get; set; }
}
