using System.Reflection;
using Microsoft.EntityFrameworkCore;
using myEVote.Domain.Entities;

namespace myEVote.Infraestructure.Persistence.Context;

public class MyEVoteContext : DbContext
{
    public MyEVoteContext(DbContextOptions<MyEVoteContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Ciudadano> Ciudadanos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<PuestoElectivo> PuestosElectivos { get; set; }
    public DbSet<PartidoPolitico> PartidosPoliticos { get; set; }
    public DbSet<Candidato> Candidatos { get; set; }
    public DbSet<CandidatoPuesto> CandidatoPuestos { get; set; }
    public DbSet<DirigentePartido> DirigentePartidos { get; set; }
    public DbSet<AlianzaPolitica> AlianzasPoliticas { get; set; }
    public DbSet<SolicitudAlianza> SolicitudesAlianza { get; set; }
    public DbSet<Eleccion> Elecciones { get; set; }
    public DbSet<Voto> Votos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}