using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class Eleccion : BaseEntity
{
    public string Nombre { get; set; }
    public DateTime FechaEleccion { get; set; }
    public EstadoEleccion EstadoEleccion { get; set; }
    
    public ICollection<Voto>? Votos { get; set; }
}
