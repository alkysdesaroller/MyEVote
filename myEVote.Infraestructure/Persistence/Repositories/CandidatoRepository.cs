using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class CandidatoRepository(MyEVoteContext context) : GenericRepository<Candidato>(context), ICandidatoRespository
{
    private readonly MyEVoteContext _context = context;

    //obtiene acivos por partido politico
    public async Task<List<Candidato>> GetAllActivosAsync(int partidoPoliticoId)
    {
        return await _context.Candidatos
            .Include(c => c.PartidoPolitico)
            .Where(c => c.PartidoPoliticoId == partidoPoliticoId)
            .ToListAsync();
    }

    //obtiene todos segun el ID del partido
    public async Task<List<Candidato>> GetActivosByPartidoPoliticoIdAsync(int partidoPoliticoId)
    {
        return await _context.Candidatos
            .Include(c => c.PartidoPolitico)
            .Where(c => c.PartidoPoliticoId == partidoPoliticoId && c.Estado == EstadoEntidad.Activo)
            .ToListAsync();
    }
}