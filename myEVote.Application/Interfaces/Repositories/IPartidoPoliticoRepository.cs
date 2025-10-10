using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IPartidoPoliticoRepository : IGenericRepository<PartidoPolitico>
{
    Task<List<PartidoPolitico>> GetAllActivosAsync();
    Task<bool>  ExistsBySiglasAsync(string siglas, int? excludeId = null);
    
}