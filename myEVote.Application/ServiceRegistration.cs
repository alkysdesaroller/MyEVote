using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using myEVote.Application.Interfaces.Services;
using myEVote.Application.Services;

namespace myEVote.Application;

public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        #region AutoMapper
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        #endregion
        
        #region Services
        // Servicio Genérico
        services.AddTransient(typeof(IGenericService<,>), typeof(GenericService<,,>));

        // Servicios Específicos
        services.AddTransient<ICiudadanoService, CiudadanoService>();
        services.AddTransient<IUsuarioService, UsuarioService>();
        services.AddTransient<IPuestoElectivoService, PuestoElectivoService>();
        services.AddTransient<IPartidoPoliticoService, PartidoPoliticoService>();
        services.AddTransient<ICandidatoService, CandidatoService>();
        services.AddTransient<ICandidatoPuestoService, CandidatoPuestoService>();
        services.AddTransient<IDirigentePartidoService, DirigentePartidoService>();
        services.AddTransient<IAlianzaPoliticaService, AlianzaPoliticaService>();
        services.AddTransient<ISolicitudAlianzaService, SolicitudAlianzaService>();
        services.AddTransient<IEleccionService, EleccionService>();
        services.AddTransient<IVotoService, VotoService>();
        services.AddTransient<IAccountService, AccountService>();
        
        #endregion
    }
}