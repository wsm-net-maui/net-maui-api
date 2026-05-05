using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class HorarioAtendimentoConfiguration : IEntityTypeConfiguration<HorarioAtendimento>
{
    public void Configure(EntityTypeBuilder<HorarioAtendimento> builder)
    {
        builder.ToTable("HorariosAtendimento");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.DiaSemana).IsRequired();
        builder.Property(h => h.HoraInicio).IsRequired();
        builder.Property(h => h.HoraFim).IsRequired();
        builder.Property(h => h.IntervaloMinutos).IsRequired();
        builder.Property(h => h.Ativo).IsRequired();
        builder.Property(h => h.CriadoEm).IsRequired();
        builder.Property(h => h.AtualizadoEm);

        builder.HasOne(h => h.Servico)
               .WithMany()
               .HasForeignKey(h => h.ServicoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
