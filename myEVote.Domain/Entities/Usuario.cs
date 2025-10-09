using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RolUsuario RolUsuario { get; set; }
    public EstadoEntidad Estado { get; set; }
    
    //para navegar eh.
    public DirigentePartido? DirigentePartido { get; set; }
}