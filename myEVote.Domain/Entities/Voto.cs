using myEVote.Domain.Common;

namespace myEVote.Domain.Entities;

public class Voto : BaseEntity
{
    public int CiudadanoId { get; set; }
    public int CandidatoId { get; set; }
    public int PuestoElectivoId { get; set; }
    public int EleccionId { get; set; }
    public DateTime FechaVoto { get; set; }

    // Navegación
    public Ciudadano? Ciudadano { get; set; }
    public Candidato? Candidato { get; set; }
    public PuestoElectivo? PuestoElectivo { get; set; }
    public Eleccion? Eleccion { get; set; }
    
}