using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace myEVote.ViewModel.Admin;

public class SaveDirigentePartidoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un dirigente político")]
    [Display(Name = "Dirigente Político")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un partido político")]
    [Display(Name = "Partido Político")]
    public int PartidoPoliticoId { get; set; }

    public List<SelectListItem> Usuarios { get; set; } = new();
    public List<SelectListItem> PartidosPoliticos { get; set; } = new();
}