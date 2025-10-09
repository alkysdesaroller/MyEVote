using myEVote.Domain.Common;
using myEVote.Domain.Enums;

namespace myEVote.Domain.Entities;

public class PartidoPolitico : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Siglas { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public EstadoEntidad Estado { get; set; }
    
    // para navegación
    public ICollection<Candidato> Candidatos { get; set; }
    public ICollection<DirigentePartido> DirigentePartidos { get; set; }
    public ICollection<AlianzaPolitica> AlianzasComoPartido1 { get; set; }
    public ICollection<AlianzaPolitica> AlianzasComoPartido2 { get; set; }
    public ICollection<SolicitudAlianza> SolicitudesEnviadas { get; set; }
    public ICollection<SolicitudAlianza> SolicitudesRecibidas { get; set; }
}