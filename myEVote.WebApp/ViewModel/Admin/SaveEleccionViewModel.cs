using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Admin;

public class SaveEleccionViewModel
{
    [Required(ErrorMessage = "El nombre de la elección es obligatorio")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    [Display(Name = "Nombre de la Elección")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [Display(Name = "Fecha de Realización")]
    [DataType(DataType.Date)]
    public DateTime FechaRealizacion { get; set; } = DateTime.Now;

    public List<string> ErrorMessages { get; set; } = new();
    public bool HasErrors => ErrorMessages.Any();
}