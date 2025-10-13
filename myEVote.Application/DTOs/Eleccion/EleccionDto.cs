using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.Eleccion;

public class EleccionDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaRealizacion { get; set; }
    public EstadoEleccion Estado { get; set; }
    public int CantidadPartidos { get; set; }
    public int CantidadCandidatos { get; set; }
    public int CantidadPuestos { get; set; }
    public int TotalVotos { get; set; }
    public bool EstaActiva => Estado == EstadoEleccion.EnProceso;
}