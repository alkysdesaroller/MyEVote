using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class AlianzaPoliticaConfiguration : IEntityTypeConfiguration<AlianzaPolitica>
{
    public void Configure(EntityTypeBuilder<AlianzaPolitica> builder)
    {
        builder.ToTable("AlianzasPoliticas");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaAlianza)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relaciones
        builder.HasOne(e => e.PartidoPolitico1)
            .WithMany(p => p.AlianzasComoPartido1)
            .HasForeignKey(e => e.PartidoPolitico1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartidoPolitico2)
            .WithMany(p => p.AlianzasComoPartido2)
            .HasForeignKey(e => e.PartidoPolitico2Id)
            .OnDelete(DeleteBehavior.Restrict);

        // No puede haber alianza duplicada entre los mismos partidos
        builder.HasIndex(e => new { e.PartidoPolitico1Id, e.PartidoPolitico2Id })
            .IsUnique();
    }
}