using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IEleccionRepository : IGenericRepository<Eleccion>
{
    Task<Eleccion> GetEleccionActivaAsync();
    Task<bool> ExistsEleccionActivaAsync();
    Task<List<Eleccion>> GetAllOrderedByDateDescAsync();
}