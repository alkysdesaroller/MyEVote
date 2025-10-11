using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace myEVote.Application;

public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}