using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Admin;

public class SavePuestoElectivoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del puesto es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Display(Name = "Nombre del Puesto")]
    public string? Nombre { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }
}