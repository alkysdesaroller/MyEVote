namespace myEVote.Application.DTOs.AlianzaPolitica;

public class AlianzaPoliticaDto
{
    public int Id { get; set; }
    public int PartidoPolitico1Id { get; set; }
    public string PartidoPolitico1Nombre { get; set; } = string.Empty;
    public string PartidoPolitico1Siglas { get; set; } = string.Empty;
    public int PartidoPolitico2Id { get; set; }
    public string PartidoPolitico2Nombre { get; set; } = string.Empty;
    public string PartidoPolitico2Siglas { get; set; } = string.Empty;
    public DateTime FechaAlianza { get; set; }
}