using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class PartidoPoliticoConiguration : IEntityTypeConfiguration<PartidoPolitico>
{
    public void Configure(EntityTypeBuilder<PartidoPolitico> builder)
    {
        builder.ToTable("PartidosPoliticos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.Descripcion)
            .HasMaxLength(500);

        builder.Property(e => e.Siglas)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(e => e.Siglas)
            .IsUnique();

        builder.Property(e => e.LogoUrl)
            .HasMaxLength(500);

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();
    }
}