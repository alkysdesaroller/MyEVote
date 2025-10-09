using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class CiudadanoConfiguration : IEntityTypeConfiguration<Ciudadano>
{
    public void Configure(EntityTypeBuilder<Ciudadano> builder)
    {
        builder.ToTable("Ciudadanos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Cedula)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(e => e.Cedula)
            .IsUnique();

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();
    }
}