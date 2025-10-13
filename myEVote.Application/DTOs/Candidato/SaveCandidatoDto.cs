namespace myEVote.Application.DTOs.Candidato;

public class SaveCandidatoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string FotoUrl { get; set; } = string.Empty;
    public int PartidoPoliticoId { get; set; } 
}