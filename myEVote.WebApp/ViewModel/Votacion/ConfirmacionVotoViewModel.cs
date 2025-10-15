namespace myEVote.ViewModel.Votacion;

public class ConfirmacionVotoViewModel
{
    public string? EleccionNombre { get; set; }
    public string? CiudadanoNombre { get; set; }
    public List<ResumenVotoViewModel> VotosRealizados { get; set; } = new();
    public bool ProcesoCompleto { get; set; }
    public List<string> PuestosFaltantes { get; set; } = new();
}

public class ResumenVotoViewModel
{
    public string? PuestoNombre { get; set; }
    public string? CandidatoNombre { get; set; }
    public string? PartidoSiglas { get; set; }
}