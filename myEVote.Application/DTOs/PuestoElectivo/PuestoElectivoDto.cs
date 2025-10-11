using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.PuestoElectivo;

public class PuestoElectivoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
}