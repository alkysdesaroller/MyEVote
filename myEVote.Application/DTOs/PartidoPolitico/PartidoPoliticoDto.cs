using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.PartidoPolitico;

public class PartidoPoliticoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
}