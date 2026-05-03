using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class ClientePerfilConfiguration : IEntityTypeConfiguration<ClientePerfil>
{
    public void Configure(EntityTypeBuilder<ClientePerfil> builder)
    {
        builder.ToTable("ClientePerfis");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Telefone)
            .HasMaxLength(20);

        builder.Property(c => c.Observacoes)
            .HasMaxLength(1000);

        builder.Property(c => c.Ativo).IsRequired();
        builder.Property(c => c.CriadoEm).IsRequired();
        builder.Property(c => c.AtualizadoEm);

        builder.HasOne(c => c.Usuario)
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
