using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class CandidatoConfiguration : IEntityTypeConfiguration<Candidato>
{
    public void Configure(EntityTypeBuilder<Candidato> builder)
    {
        builder.ToTable("Candidatos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.FotoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relación con PartidoPolitico
        builder.HasOne(e => e.PartidoPolitico)
            .WithMany(p => p.Candidatos)
            .HasForeignKey(e => e.PartidoPoliticoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}