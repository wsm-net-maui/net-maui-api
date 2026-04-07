using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.Property(p => p.CodigoBarras)
            .HasMaxLength(50);

        builder.HasIndex(p => p.CodigoBarras);

        builder.Property(p => p.PrecoCompra)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.PrecoVenda)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.QuantidadeEstoque)
            .IsRequired();

        builder.Property(p => p.EstoqueMinimo)
            .IsRequired();

        builder.Property(p => p.CategoriaId)
            .IsRequired();

        builder.Property(p => p.Ativo)
            .IsRequired();

        builder.Property(p => p.CriadoEm)
            .IsRequired();

        builder.Property(p => p.AtualizadoEm);

        builder.HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Movimentacoes)
            .WithOne(m => m.Produto)
            .HasForeignKey(m => m.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
