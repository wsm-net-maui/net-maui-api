using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Descricao)
            .HasMaxLength(500);

        builder.Property(s => s.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.DuracaoMinutos).IsRequired();
        builder.Property(s => s.Ativo).IsRequired();
        builder.Property(s => s.CriadoEm).IsRequired();
        builder.Property(s => s.AtualizadoEm);
    }
}
