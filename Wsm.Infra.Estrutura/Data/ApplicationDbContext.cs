using Microsoft.EntityFrameworkCore;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<CarrinhoItem> CarrinhoItens => Set<CarrinhoItem>();
    public DbSet<FuncionarioPerfil> FuncionarioPerfis => Set<FuncionarioPerfil>();
    public DbSet<ClientePerfil> ClientePerfis => Set<ClientePerfil>();
    public DbSet<Servico> Servicos => Set<Servico>();
    public DbSet<HorarioAtendimento> HorariosAtendimento => Set<HorarioAtendimento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
