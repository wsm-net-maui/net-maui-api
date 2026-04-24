using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Numero)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(p => p.Numero)
            .IsUnique();

        builder.Property(p => p.UsuarioId)
            .IsRequired();

        builder.Property(p => p.VoucherId);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.ValorBruto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.ValorDesconto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.ValorTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Ativo)
            .IsRequired();

        builder.Property(p => p.CriadoEm)
            .IsRequired();

        builder.Property(p => p.AtualizadoEm);

        builder.HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Voucher)
            .WithMany()
            .HasForeignKey(p => p.VoucherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Itens)
            .WithOne(i => i.Pedido)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Itens)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.UsuarioId);
        builder.HasIndex(p => p.VoucherId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CriadoEm);
    }
}
