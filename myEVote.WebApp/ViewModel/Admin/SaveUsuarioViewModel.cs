using System.ComponentModel.DataAnnotations;
using myEVote.Domain.Enums;

namespace myEVote.ViewModel.Admin;

public class SaveUsuarioViewModel(int id)
{
    public int Id { get; set; } = id;

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

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")]
    [Display(Name = "Nombre de Usuario")]
    public string? NombreUsuario { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string? Contrasena { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Contraseña")]
    [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
    public string? ConfirmarContrasena { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio")]
    [Display(Name = "Rol")]
    public RolUsuario Rol { get; set; }

    public bool IsEdit => Id > 0;
}