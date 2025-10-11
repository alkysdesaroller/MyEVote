using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.Usuario;

public class UsuarioDto
{
    

    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;  
    public string Apellido { get; set; } = string.Empty;  
    public string Email { get; set; } = string.Empty;  
    public string NombreUsuario { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; } 
    public EstadoEntidad Estado { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string RolNombre => Rol == RolUsuario.Administrador ? "Administrador" : "Dirigente Político";
}