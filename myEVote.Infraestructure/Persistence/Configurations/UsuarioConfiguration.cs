using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
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

        builder.Property(e => e.NombreUsuario)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.NombreUsuario)
            .IsUnique();

        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.RolUsuario)
            .IsRequired();

        builder.Property(e => e.Estado)
            .IsRequired();

        builder.Property(e => e.FechaCreacion)
            .IsRequired();
    }
}