using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Admin;

public class SavePartidoPoliticoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del partido es obligatorio")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    [Display(Name = "Nombre del Partido")]
    public string? Nombre { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Las siglas son obligatorias")]
    [StringLength(20, ErrorMessage = "Las siglas no pueden exceder 20 caracteres")]
    [Display(Name = "Siglas")]
    public string? Siglas { get; set; }

    [Display(Name = "Logo del Partido")]
    public IFormFile? Logo { get; set; }

    public string? LogoUrl { get; set; }

    public bool HasExistingLogo => !string.IsNullOrEmpty(LogoUrl);
}