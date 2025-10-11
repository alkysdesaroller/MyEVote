using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class AlianzaPoliticaRepository(MyEVoteContext context) : GenericRepository<AlianzaPolitica>(context), IAlianzaPoliticaRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<List<AlianzaPolitica>> GetByPartidoAsync(int partidoId)
    {
        return await _context.AlianzasPoliticas
            .Include(ap => ap.PartidoPolitico1)
            .Include(ap => ap.PartidoPolitico2)
            .Where(ap => ap.PartidoPolitico1Id == partidoId || ap.PartidoPolitico2Id == partidoId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAlianzaAsync(int partido1Id, int partido2Id)
    {
        return await _context.AlianzasPoliticas
            .AnyAsync(a => (a.PartidoPolitico1Id == partido1Id && a.PartidoPolitico2Id == partido2Id)
                           || (a.PartidoPolitico1Id == partido2Id && a.PartidoPolitico2Id == partido1Id));
    }
}