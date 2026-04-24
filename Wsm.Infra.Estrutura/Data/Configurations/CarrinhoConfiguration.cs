using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class CarrinhoConfiguration : IEntityTypeConfiguration<Carrinho>
{
    public void Configure(EntityTypeBuilder<Carrinho> builder)
    {
        builder.ToTable("Carrinhos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.HasIndex(c => c.UsuarioId)
            .IsUnique();

        builder.Property(c => c.VoucherId);

        builder.Property(c => c.ValorBruto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.ValorDesconto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.ValorTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder.Property(c => c.CriadoEm)
            .IsRequired();

        builder.Property(c => c.AtualizadoEm);

        builder.HasOne(c => c.Usuario)
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Voucher)
            .WithMany()
            .HasForeignKey(c => c.VoucherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Itens)
            .WithOne(i => i.Carrinho)
            .HasForeignKey(i => i.CarrinhoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Itens)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(c => c.VoucherId);
        builder.HasIndex(c => c.CriadoEm);
    }
}
