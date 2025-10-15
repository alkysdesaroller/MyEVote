using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Application.Interfaces.Services;
using myEVote.Infraestructure.Persistence.Context;
using myEVote.Infraestructure.Persistence.Repositories;
using myEVote.Infraestructure.Shared.Services;

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
            
        // Repositorios Específicos
        services.AddTransient<ICiudadanoRepository, CiudadanoRepository>();
        services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        services.AddTransient<IPuestoElectivoRepository, PuestoElectivoRepository>();
        services.AddTransient<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
        services.AddTransient<ICandidatoRepository, CandidatoRepository>();
        services.AddTransient<ICandidatoPuestoRepository, CandidatoPuestoRepository>();
        services.AddTransient<IDirigentePartidoRepository, DirigentePartidoRepository>();
        services.AddTransient<IAlianzaPoliticaRepository, AlianzaPoliticaRepository>();
        services.AddTransient<ISolicitudAlianzaRepository, SolicitudAlianzaRepository>();
        services.AddTransient<IEleccionRepository, EleccionRepository>();
        services.AddTransient<IVotoRepository, VotoRepository>();
        

        #endregion

        #region Shared 
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IOcrService, OcrService>();
        services.AddTransient<IFileUploadService, FileUploadService>();
        #endregion
    }
}