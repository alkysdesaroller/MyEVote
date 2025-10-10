using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface ISolicitudAlianzaRepository : IGenericRepository<SolicitudAlianza>
{
    Task<List<SolicitudAlianza>> GetPendientesByPartidoReceptorAsync(int partidoReceptorId);
    Task<List<SolicitudAlianza>> GetByPartidoSolicitanteAsync(int partidoSolicitanteId);
    Task<bool> ExistsSolicitudPendienteAsync(int partidoSolicitanteId, int partidoReceptorId);
}