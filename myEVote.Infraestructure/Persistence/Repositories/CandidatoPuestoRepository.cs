using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class CandidatoPuestoRepository(MyEVoteContext context)
    : GenericRepository<CandidatoPuesto>(context), ICandidatoPuestoRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<List<CandidatoPuesto>> GetActivosByPartidoIdAsync(int partidoPoliticoId)
    {
        return await _context.CandidatoPuestos
            .Include(cp => cp.Candidato)
            .Include(cp => cp.PuestoElectivo)
            .Include(cp => cp.PartidoPolitico)
            .Where(cp => cp.PartidoPoliticoId == partidoPoliticoId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoPoliticoId)
    {
        return await _context.CandidatoPuestos
            .AnyAsync(cp => cp.CandidatoId == candidatoId
                            && cp.PuestoElectivoId == puestoElectivoId
                            && cp.PartidoPoliticoId == partidoPoliticoId);
    }

    public async Task<CandidatoPuesto> GetByCandidatoPuestoAsync(int candidatoId, int puestoElectivoId)
    {
        return await _context.CandidatoPuestos
                   .Include(cp => cp.Candidato)
                   .Include(cp => cp.PuestoElectivo)
                   .Include(cp => cp.PartidoPolitico)
                   .FirstOrDefaultAsync(cp =>
                       cp.CandidatoId == candidatoId && cp.PuestoElectivoId == puestoElectivoId) ??
               throw new InvalidOperationException();
    }
}