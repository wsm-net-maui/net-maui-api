namespace Wsm.Domain.Entities;

public class Servico : BaseEntity
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public decimal Preco { get; private set; }
    public int DuracaoMinutos { get; private set; }

    private Servico() { }

    public Servico(string nome, decimal preco, int duracaoMinutos, string? descricao = null)
    {
        Nome = nome;
        Preco = preco;
        DuracaoMinutos = duracaoMinutos;
        Descricao = descricao;
    }

    public void Atualizar(string nome, decimal preco, int duracaoMinutos, string? descricao)
    {
        Nome = nome;
        Preco = preco;
        DuracaoMinutos = duracaoMinutos;
        Descricao = descricao;
        AtualizarDataModificacao();
    }
}
