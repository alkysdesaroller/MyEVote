using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure;

public static class ServiceRegistration
{
    public static void AddPersistenceInfraesctruture(this IServiceCollection services, IConfiguration configuration)
    {
        //DbContext E'te ejel regitro😣😣😣😣
        services.AddDbContext<MyEVoteContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MyEVoteContext).Assembly.FullName)
            )
        );
    }
}