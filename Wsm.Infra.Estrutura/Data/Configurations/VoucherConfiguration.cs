using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.ToTable("Vouchers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Codigo)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(v => v.Codigo)
            .IsUnique();

        builder.Property(v => v.TipoDesconto)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(v => v.ValorDesconto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(v => v.ValorMinimoPedido)
            .HasPrecision(18, 2);

        builder.Property(v => v.DataInicio)
            .IsRequired();

        builder.Property(v => v.DataFim)
            .IsRequired();

        builder.Property(v => v.UsoMaximo);

        builder.Property(v => v.UsoAtual)
            .IsRequired();

        builder.Property(v => v.Ativo)
            .IsRequired();

        builder.Property(v => v.CriadoEm)
            .IsRequired();

        builder.Property(v => v.AtualizadoEm);

        builder.HasIndex(v => v.Ativo);
        builder.HasIndex(v => v.DataInicio);
        builder.HasIndex(v => v.DataFim);
    }
}
