using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class PuestoElectivo : BaseEntity    
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
    
    //para navegacion
    public ICollection<CandidatoPuesto>? CandidatoPuestos { get; set; }
}