using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.Usuario;

public class SaveUsuarioDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
}