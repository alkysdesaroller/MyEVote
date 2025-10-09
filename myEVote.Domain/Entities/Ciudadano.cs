using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class Ciudadano : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }

    // Navegación
    public ICollection<Voto>? Votos { get; set; }
}