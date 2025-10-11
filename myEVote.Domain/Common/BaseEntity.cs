namespace myEVote.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string Creador { get; set;  } = String.Empty;
    public string Modificador { get; set; } = String.Empty;
}