using myEVote.Domain.Common;

namespace myEVote.Domain.Entities;

public class AlianzaPolitica : BaseEntity
{
    public int PartidoPolitico1Id { get; set; }
    public int PartidoPolitico2Id { get; set; }
    public DateTime FechaAlianza { get; set; }
     
    //Para Navegacion
    public PartidoPolitico? PartidoPolitico1 { get; set; }
    public PartidoPolitico? PartidoPolitico2 { get; set; }
}