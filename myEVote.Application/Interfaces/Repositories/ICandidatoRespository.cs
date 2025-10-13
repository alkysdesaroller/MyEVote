using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface ICandidatoRepository : IGenericRepository<Candidato>
{
    Task<List<Candidato>> GetAllActivosAsync(int partidoPoliticoId);
    Task<List<Candidato>> GetActivosByPartidoPoliticoIdAsync(int partidoPoliticoId);
    
}