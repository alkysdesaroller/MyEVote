using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class Candidato : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string FotoUrl { get; set; } = string.Empty;
    public int PartidoPoliticoId { get; set; } 
    public EstadoEntidad Estado { get; set; }
    
    //para navegacion 
    public PartidoPolitico? PartidoPolitico { get; set; }
    public ICollection<CandidatoPuesto>? CandidatoPuestos { get; set; }
    public ICollection<Voto>? Votos { get; set; }
}