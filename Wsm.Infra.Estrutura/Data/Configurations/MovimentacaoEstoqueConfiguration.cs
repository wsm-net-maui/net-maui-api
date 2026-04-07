using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wsm.Domain.Entities;

namespace Wsm.Infra.Estrutura.Data.Configurations;

public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.ToTable("MovimentacoesEstoque");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ProdutoId)
            .IsRequired();

        builder.Property(m => m.Tipo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.Quantidade)
            .IsRequired();

        builder.Property(m => m.Observacao)
            .HasMaxLength(500);

        builder.Property(m => m.UsuarioId)
            .IsRequired();

        builder.Property(m => m.Ativo)
            .IsRequired();

        builder.Property(m => m.CriadoEm)
            .IsRequired();

        builder.Property(m => m.AtualizadoEm);

        builder.HasOne(m => m.Produto)
            .WithMany(p => p.Movimentacoes)
            .HasForeignKey(m => m.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Usuario)
            .WithMany()
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => m.ProdutoId);
        builder.HasIndex(m => m.UsuarioId);
        builder.HasIndex(m => m.CriadoEm);
    }
}
