namespace myEVote.Application.DTOs.Voto;

public class SaveVotoDto
{
    public int CiudadanoId { get; set; }
    public int CandidatoId { get; set; }
    public int PuestoElectivoId { get; set; }
    public int EleccionId { get; set; }
}