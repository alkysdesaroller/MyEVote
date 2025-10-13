namespace myEVote.Application.DTOs.CandidatoPuesto;

public class CandidatoPuestoDto
{
    public int Id { get; set; }
    public int CandidatoId { get; set; }
    public string CandidatoNombre { get; set; } = string.Empty;
    public string CandidatoApellido { get; set; } = string.Empty;
    public string CandidatoFotoUrl { get; set; } = string.Empty;
    public int PuestoElectivoId { get; set; }
    public string PuestoElectivoNombre { get; set; } = string.Empty; 
    public int PartidoPoliticoId { get; set; } 
    public string PartidoPoliticoNombre { get; set; } = string.Empty;
    public string PartidoPoliticoSiglas { get; set; } = string.Empty;
    public string CandidatoNombreCompleto => $"{CandidatoNombre} {CandidatoApellido}";
}