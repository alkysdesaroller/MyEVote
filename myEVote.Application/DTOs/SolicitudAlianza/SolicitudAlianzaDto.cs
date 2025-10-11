using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.SolicitudAlianza;

public class SolicitudAlianzaDto
{
    public int Id { get; set; }
    public int PartidoSolicitanteId { get; set; }
    public string PartidoSolicitanteNombre { get; set; } = string.Empty;
    public string PartidoSolicitanteSiglas { get; set; } = string.Empty;
    public int PartidoReceptorId { get; set; }
    public string PartidoReceptorNombre { get; set; } = string.Empty;
    public string PartidoReceptorSiglas { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public EstadoSolicitud Estado { get; set; }
    public string EstadoNombre => Estado switch
    {
        EstadoSolicitud.EnProceso => "En Espera de Respuesta",
        EstadoSolicitud.Aceptado => "Aceptada",
        EstadoSolicitud.Rechazado => "Rechazada",
        _ => "Desconocido"
    };
}