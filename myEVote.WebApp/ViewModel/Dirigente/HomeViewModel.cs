namespace myEVote.ViewModel.Dirigente;

public class HomeViewModel
{
    public string PartidoNombre { get; set; } = string.Empty;
    public string PartidoSiglas { get; set; } = string.Empty;
    public string PartidoLogoUrl { get; set; } = string.Empty;
    public int CantidadCandidatosActivos { get; set; }
    public int CantidadCandidatosInactivos { get; set; }
    public int CantidadAlianzas { get; set; }
    public int CantidadSolicitudesPendientes { get; set; }
    public int CantidadCandidatosAsignados { get; set; }
}