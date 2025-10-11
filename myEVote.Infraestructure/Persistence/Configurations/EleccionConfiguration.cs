using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class EleccionConfiguration : IEntityTypeConfiguration<Eleccion>
{
    public void Configure(EntityTypeBuilder<Eleccion> builder)
    {
        builder.ToTable("Elecciones");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.FechaEleccion)
            .IsRequired();

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();
    }
}