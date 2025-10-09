using myEVote.Domain.Common;

namespace myEVote.Domain.Entities;

public class DirigentePartido : BaseEntity
{
    public int UsuarioId { get; set; }
    public int PartidoPoliticoId { get; set; }

    // Navegación
    public Usuario? Usuario { get; set; }
    public PartidoPolitico? PartidoPolitico { get; set; }
}