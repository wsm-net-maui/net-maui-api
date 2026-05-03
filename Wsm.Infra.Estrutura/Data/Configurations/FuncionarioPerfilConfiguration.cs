using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class FuncionarioPerfilConfiguration : IEntityTypeConfiguration<FuncionarioPerfil>
{
    public void Configure(EntityTypeBuilder<FuncionarioPerfil> builder)
    {
        builder.ToTable("FuncionarioPerfis");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Especialidade)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Descricao)
            .HasMaxLength(500);

        builder.Property(f => f.Ativo).IsRequired();
        builder.Property(f => f.CriadoEm).IsRequired();
        builder.Property(f => f.AtualizadoEm);

        builder.HasOne(f => f.Usuario)
            .WithMany()
            .HasForeignKey(f => f.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Horarios)
            .WithOne(h => h.FuncionarioPerfil)
            .HasForeignKey(h => h.FuncionarioPerfilId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
