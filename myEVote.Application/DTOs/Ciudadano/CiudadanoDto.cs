using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs;

public class CiudadanoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}"; 
}