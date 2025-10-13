namespace myEVote.Application.DTOs.Eleccion;

public class ResultadoPuestoDto
{
    public int PuestoElectivoId { get; set; }
    public string PuestoElectivoNombre { get; set; } = string.Empty;
    public int TotalVotos { get; set; }
    public List<ResultadoCandidatoDto>? Candidatos { get; set; }
}