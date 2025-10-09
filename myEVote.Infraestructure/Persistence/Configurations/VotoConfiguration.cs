using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class VotoConfiguration : IEntityTypeConfiguration<Voto>
{
    public void Configure(EntityTypeBuilder<Voto> builder)
    {
        builder.ToTable("Votos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaVoto)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relaciones
        builder.HasOne(e => e.Ciudadano)
            .WithMany(c => c.Votos)
            .HasForeignKey(e => e.CiudadanoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Candidato)
            .WithMany(c => c.Votos)
            .HasForeignKey(e => e.CandidatoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PuestoElectivo)
            .WithMany()
            .HasForeignKey(e => e.PuestoElectivoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Eleccion)
            .WithMany(e => e.Votos)
            .HasForeignKey(e => e.EleccionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Un ciudadano solo puede votar una vez por puesto en una elección
        builder.HasIndex(e => new { e.CiudadanoId, e.PuestoElectivoId, e.EleccionId })
            .IsUnique();
    }
}