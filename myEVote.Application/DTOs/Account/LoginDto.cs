namespace myEVote.Application.DTOs.Account;

public class LoginDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int? PartidoPoliticoId { get; set; }
    public string PartidoPoliticoNombre { get; set; } = string.Empty;
    public string PartidoPoliticoSiglas { get; set; } = string.Empty;
    public string PartidoPoliticoLogoUrl { get; set; } = string.Empty;
    public bool TienePartidoAsignado => PartidoPoliticoId.HasValue;
}