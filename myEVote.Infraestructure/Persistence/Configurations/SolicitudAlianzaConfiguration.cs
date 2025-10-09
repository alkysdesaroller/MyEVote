using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class SolicitudAlianzaConfiguration : IEntityTypeConfiguration<SolicitudAlianza>
{
    public void Configure(EntityTypeBuilder<SolicitudAlianza> builder)
    {
        builder.ToTable("SolicitudesAlianza");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaSolicitud)
            .IsRequired();

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relaciones
        builder.HasOne(e => e.PartidoSolicitante)
            .WithMany(p => p.SolicitudesEnviadas)
            .HasForeignKey(e => e.PartidoSolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartidoReceptor)
            .WithMany(p => p.SolicitudesRecibidas)
            .HasForeignKey(e => e.PartidoReceptorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}