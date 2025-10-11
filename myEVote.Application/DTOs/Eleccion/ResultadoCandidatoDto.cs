namespace myEVote.Application.DTOs.Eleccion;

public class ResultadoCandidatoDto
{
    public int CandidatoId { get; set; }
    public string CandidatoNombre { get; set; }  = string.Empty;
    public string CandidatoApellido { get; set; } = string.Empty;
    public string CandidatoFotoUrl { get; set; } = string.Empty;
    public string PartidoPoliticoNombre { get; set; } = string.Empty;
    public string PartidoPoliticoSiglas { get; set; } = string.Empty;
    public string PartidoPoliticoLogoUrl { get; set; } = string.Empty;
    public int CantidadVotos { get; set; }
    public decimal PorcentajeVotos { get; set; }
    public string CandidatoNombreCompleto => $"{CandidatoNombre} {CandidatoApellido}";
}