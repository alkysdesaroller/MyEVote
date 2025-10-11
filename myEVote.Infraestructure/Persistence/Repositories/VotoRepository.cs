using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class VotoRepository(MyEVoteContext context) : GenericRepository<Voto>(context), IVotoRepository
{
    private readonly MyEVoteContext _context = context;
    
    public async Task<bool> HasVotedInEleccionAsync(int ciudadanoId, int eleccionId)
    {
        return await _context.Votos
            .AnyAsync(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId);
    }

    public async Task<List<Voto>> GetVotosByEleccionIdAsync(int eleccionId)
    {
        return await _context.Votos
            .Include(v => v.Candidato)
            .Include(v => v.PuestoElectivo)
            .Include(v => v.Ciudadano)
            .Where(v => v.EleccionId == eleccionId)
            .ToListAsync();
    }

    public async Task<List<Voto>> GetVotosByPuestoAndEleccionAsync(int puestoElectivoId, int eleccionId)
    {
        return await _context.Votos
            .Include(v => v.Candidato)
            .ThenInclude(c => c!.PartidoPolitico)
            .Include(v => v.PuestoElectivo)
            .Where(v => v.PuestoElectivoId == puestoElectivoId && v.EleccionId == eleccionId)
            .ToListAsync();
    }
}