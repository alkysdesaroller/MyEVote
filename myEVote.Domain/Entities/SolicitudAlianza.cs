using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class SolicitudAlianza : BaseEntity
{
    public int PartidoSolicitanteId { get; set; }
    public int PartidoReceptorId { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public EstadoSolicitud Estado { get; set; }

    // Navegación
    public PartidoPolitico? PartidoSolicitante { get; set; }
    public PartidoPolitico? PartidoReceptor { get; set; }
}