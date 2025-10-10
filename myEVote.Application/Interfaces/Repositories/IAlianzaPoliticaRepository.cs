using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IAlianzaPoliticaRepository : IGenericRepository<AlianzaPolitica>
{
    Task<List<AlianzaPolitica>> GetByPartidoAsync(int partidoId);
    Task<bool> ExistsAlianzaAsync(int partido1Id, int partido2Id);
    
}