using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class CarrinhoItemConfiguration : IEntityTypeConfiguration<CarrinhoItem>
{
    public void Configure(EntityTypeBuilder<CarrinhoItem> builder)
    {
        builder.ToTable("CarrinhoItens");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.CarrinhoId)
            .IsRequired();

        builder.Property(i => i.ProdutoId)
            .IsRequired();

        builder.Property(i => i.ProdutoNome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Quantidade)
            .IsRequired();

        builder.Property(i => i.PrecoUnitario)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Ativo)
            .IsRequired();

        builder.Property(i => i.CriadoEm)
            .IsRequired();

        builder.Property(i => i.AtualizadoEm);

        builder.HasOne(i => i.Carrinho)
            .WithMany(c => c.Itens)
            .HasForeignKey(i => i.CarrinhoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.CarrinhoId);
        builder.HasIndex(i => i.ProdutoId);
        builder.HasIndex(i => new { i.CarrinhoId, i.ProdutoId });
    }
}
