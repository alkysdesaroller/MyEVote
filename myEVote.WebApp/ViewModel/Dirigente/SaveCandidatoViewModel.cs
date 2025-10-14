using System.ComponentModel.DataAnnotations;

namespace myEVote.ViewModel.Dirigente;

public class SaveCandidatoViewModel
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

    [Display(Name = "Foto del Candidato")]
    public IFormFile? Foto { get; set; }

    public string? FotoUrl { get; set; }

    public int PartidoPoliticoId { get; set; }

    public bool HasExistingFoto => !string.IsNullOrEmpty(FotoUrl);
    public bool IsEdit => Id > 0;
}