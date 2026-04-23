using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.ToTable("PedidoItens");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.PedidoId)
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

        builder.HasOne(i => i.Pedido)
            .WithMany(p => p.Itens)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Produto)
            .WithMany()
            .HasForeignKey(i => i.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.PedidoId);
        builder.HasIndex(i => i.ProdutoId);
    }
}
