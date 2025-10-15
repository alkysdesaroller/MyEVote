using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Votacion;

public class ValidacionIdentidadViewModel
{
    public string? Cedula { get; set; }
    
    [Required(ErrorMessage = "Debe subir una foto de su cédula")]
    [Display(Name = "Foto de Cédula")]
    public IFormFile? FotoCedula { get; set; }

    public string? ErrorMessage { get; set; }
}