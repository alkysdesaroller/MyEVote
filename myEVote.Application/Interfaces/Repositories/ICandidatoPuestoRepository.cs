using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface ICandidatoPuestoRepository : IGenericRepository<CandidatoPuesto>
{
    Task<List<CandidatoPuesto>> GetAllActivosAsync(int partidoPoliticoId);
    Task<bool> ExistsAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoPoliticoId);
    Task<CandidatoPuesto> GetByCandidatoPuestoAsync(int candidatoId, int puestoElectivoId);
        
}