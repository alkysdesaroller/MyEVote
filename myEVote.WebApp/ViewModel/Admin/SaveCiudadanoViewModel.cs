using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Admin;

public class SaveCiudadanoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Display(Name = "Nombre")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    [Display(Name = "Apellido")]
    public string? Apellido { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(200, ErrorMessage = "El email no puede exceder 200 caracteres")]
    [Display(Name = "Correo Electrónico")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "El documento de identidad es obligatorio")]
    [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
    [Display(Name = "Cedula")]
    [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "Formato inválido. Use: 000-0000000-0")]
    public string? Cedula { get; set; }
}