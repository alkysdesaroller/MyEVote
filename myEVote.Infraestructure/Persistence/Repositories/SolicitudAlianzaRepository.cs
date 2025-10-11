using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class SolicitudAlianzaRepository(MyEVoteContext context)
    : GenericRepository<SolicitudAlianza>(context), ISolicitudAlianzaRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<List<SolicitudAlianza>> GetPendientesByPartidoReceptorAsync(int partidoReceptorId)
    {
        return await _context.SolicitudesAlianza
            .Include(s => s.PartidoSolicitante)
            .Include(s => s.PartidoReceptor)
            .Where(s => s.PartidoReceptorId == partidoReceptorId && s.Estado == EstadoSolicitud.EnProceso)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();
    }

    public async Task<List<SolicitudAlianza>> GetByPartidoSolicitanteAsync(int partidoSolicitanteId)
    {
        return await _context.SolicitudesAlianza
            .Include(s => s.PartidoSolicitante)
            .Include(s => s.PartidoReceptor)
            .Where(s => s.PartidoSolicitanteId == partidoSolicitanteId)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();
    }

    public async Task<bool> ExistsSolicitudPendienteAsync(int partidoSolicitanteId, int partidoReceptorId)
    {
        return await _context.SolicitudesAlianza
            .AnyAsync(s => ((s.PartidoSolicitanteId == partidoSolicitanteId && s.PartidoReceptorId == partidoReceptorId)
                            || (s.PartidoSolicitanteId == partidoReceptorId &&
                                s.PartidoReceptorId == partidoSolicitanteId))
                           && s.Estado == EstadoSolicitud.EnProceso);
    }
}