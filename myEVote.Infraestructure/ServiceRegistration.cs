using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Infraestructure.Persistence.Context;
using myEVote.Infraestructure.Persistence.Repositories;

namespace myEVote.Infraestructure;

public static class ServiceRegistration
{
    public static void AddPersistenceInfraesctruture(this IServiceCollection services, IConfiguration configuration)
    {
        #region Dbcontext

//DbContext E'te ejel regitro😣😣😣😣
        services.AddDbContext<MyEVoteContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MyEVoteContext).Assembly.FullName)
            )
        );

        #endregion

        #region Repositories
        // Repositorio Genérico
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
        // Repositorios Específicos
        services.AddTransient<ICiudadanoRepository, CiudadanoRepository>();
        services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        services.AddTransient<IPuestoElectivoRespository, PuestoElectivoRepository>();
        services.AddTransient<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
        services.AddTransient<ICandidatoRespository, CandidatoRepository>();
        services.AddTransient<ICandidatoPuestoRepository, CandidatoPuestoRepository>();
        services.AddTransient<IDirigentePartidoRepository, DirigentePartidoRepository>();
        services.AddTransient<IAlianzaPoliticaRepository, AlianzaPoliticaRepository>();
        services.AddTransient<ISolicitudAlianzaRepository, SolicitudAlianzaRepository>();
        services.AddTransient<IEleccionRepository, EleccionRepository>();
        services.AddTransient<IVotoRepository, VotoRepository>();
        

        #endregion
    }
}