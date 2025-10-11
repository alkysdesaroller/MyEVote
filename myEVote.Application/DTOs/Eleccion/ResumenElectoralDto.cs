namespace myEVote.Application.DTOs.Eleccion;

public class ResumenElectoralDto
{
    public int Year { get; set; }
    public List<EleccionDto>? Elecciones { get; set; }
}