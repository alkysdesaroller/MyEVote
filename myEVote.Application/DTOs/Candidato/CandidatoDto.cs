using myEVote.Domain.Enums;

namespace myEVote.Application.DTOs.Candidato;

public class CandidatoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string FotoUrl { get; set; } = string.Empty;
    public int PartidoPoliticoId { get; set; } 
    public string PartidoPoliticoNombre { get; set; } = string.Empty;
    public string PartidoPoliticoSiglas { get; set; } = string.Empty;
    public string PartidoPoliticoLogoUrl { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}";
    public string PuestoElectivoNombre { get; set; } = string.Empty;
}