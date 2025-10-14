using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Votacion;

public class VotarCandidatoViewModel
{
    public int EleccionId { get; set; }
    public int CiudadanoId { get; set; }
    public int PuestoElectivoId { get; set; }
    public string? PuestoElectivoNombre { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un candidato")]
    public int CandidatoId { get; set; }

    public List<CandidatoVotacionViewModel> Candidatos { get; set; } = new();

    public string? ErrorMessage { get; set; }
}

public class CandidatoVotacionViewModel
{
    public int CandidatoId { get; set; }
    public string? CandidatoNombre { get; set; }
    public string? CandidatoApellido { get; set; }
    public string? CandidatoFotoUrl { get; set; }
    public string? PartidoNombre { get; set; }
    public string? PartidoSiglas { get; set; }
    public string? PartidoLogoUrl { get; set; }
    public string NombreCompleto => $"{CandidatoNombre} {CandidatoApellido}";
}
