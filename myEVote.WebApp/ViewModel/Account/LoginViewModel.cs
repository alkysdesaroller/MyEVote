using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Display(Name = "Nombre de Usuario")]
    public string NombreUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contrasena { get; set; }

    public string ErrorMessage { get; set; }
}