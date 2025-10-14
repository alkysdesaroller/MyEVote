namespace myEVote.ViewModel.Votacion;

public class ListadoPuestoViewModel
{
    public int EleccionId { get; set; }
    public string? EleccionNombre { get; set; } 
    public int CiudadanoId { get; set; }
    public List<PuestoDisponibleViewModel> Puestos { get; set; } = new();
    public List<int> PuestosYaVotados { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class PuestoDisponibleViewModel
{
    public int PuestoId { get; set; }
    public string? PuestoNombre { get; set; }
    public int CantidadPartidos { get; set; }
    public int CantidadCandidatos { get; set; }
    public bool YaVoto { get; set; }
}
