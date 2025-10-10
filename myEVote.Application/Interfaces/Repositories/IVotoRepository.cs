using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IVotoRepository : IGenericRepository<Voto>
{
    Task<bool> HasVotedInEleccionAsync(int ciudadanoId, int eleccionId);
    Task<List<Voto>> GetVotosByEleccionIdAsync(int eleccionId);
    Task<List<Voto>> GetVotosByPuestoAndEleccionAsync(int puestoElectivoId, int eleccionId);

}