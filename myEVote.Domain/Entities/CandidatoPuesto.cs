using myEVote.Domain.Common;

namespace myEVote.Domain.Entities;

public class CandidatoPuesto : BaseEntity
{
    public int CandidatoId { get; set; } 
    public int PuestoElectivoId { get; set; }
    public int PartidoPoliticoId { get; set; }

    // Navegación
    public Candidato? Candidato { get; set; }
    public PuestoElectivo? PuestoElectivo { get; set; }
    public PartidoPolitico? PartidoPolitico { get; set; }
    
}