namespace myEVote.Application.DTOs.DirigentePolitico;

public class DirigentePartidoDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string UsuarioApellido { get; set; } = string.Empty;
    public string UsuarioNombreCompleto => $"{UsuarioNombre} {UsuarioApellido}";
    public int PartidoPoliticoId { get; set; }
    public string PartidoPoliticoNombre { get; set; } = string.Empty;
    public string PartidoPoliticoSiglas { get; set; } = string.Empty;
}