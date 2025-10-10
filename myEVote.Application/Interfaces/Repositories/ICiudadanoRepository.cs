using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositorios;

public interface ICiudadanoRepository : IGenericRepository<Ciudadano>
{
    Task<Ciudadano?> GetByCedulaAsync(string cedula);
    Task<bool> ExistsByCedulaAsync(string cedula, int? excludeId = null);
    
}