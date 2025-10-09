using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class CandidatoPuestoConfiguration : IEntityTypeConfiguration<CandidatoPuesto>
{
    public void Configure(EntityTypeBuilder<CandidatoPuesto> builder)
    {
        builder.ToTable("CandidatoPuestos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relaciones
        builder.HasOne(e => e.Candidato)
            .WithMany(c => c.CandidatoPuestos)
            .HasForeignKey(e => e.CandidatoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PuestoElectivo)
            .WithMany(p => p.CandidatoPuestos)
            .HasForeignKey(e => e.PuestoElectivoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartidoPolitico)
            .WithMany()
            .HasForeignKey(e => e.PartidoPoliticoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice único: un candidato no puede estar en el mismo puesto 2 veces por el mismo partido
        builder.HasIndex(e => new { e.CandidatoId, e.PuestoElectivoId, e.PartidoPoliticoId })
            .IsUnique();
    }
}