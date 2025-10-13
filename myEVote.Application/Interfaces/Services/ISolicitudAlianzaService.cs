using myEVote.Application.DTOs.SolicitudAlianza;

namespace myEVote.Application.Interfaces.Services;

public interface ISolicitudAlianzaService
{
    Task<List<SolicitudAlianzaDto>> GetPendientesByPartidoReceptorAsync(int partidoReceptorId);
    Task<List<SolicitudAlianzaDto>> GetByPartidoSolicitanteAsync(int partidoSolicitanteId);
    Task<SolicitudAlianzaDto> AddAsync(SaveSolicitudAlianzaDto dto);
    Task AceptarAsync(int id);
    Task RechazarAsync(int id);
    Task DeleteAsync(int id);
    Task<bool> CanCreateSolicitudAsync(int partidoSolicitanteId, int partidoReceptorId);
}