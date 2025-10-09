using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Configurations;

public class DirigentePartidoConfiguration : IEntityTypeConfiguration<DirigentePartido>
{
    public void Configure(EntityTypeBuilder<DirigentePartido> builder)
    {
        builder.ToTable("DirigentePartidos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FechaCreacion)
            .IsRequired();

        // Relaciones
        builder.HasOne(e => e.Usuario)
            .WithOne(u => u.DirigentePartido)
            .HasForeignKey<DirigentePartido>(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartidoPolitico)
            .WithMany(p => p.DirigentePartidos)
            .HasForeignKey(e => e.PartidoPoliticoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Un usuario solo puede estar asignado a un partido
        builder.HasIndex(e => e.UsuarioId)
            .IsUnique();
    }
}