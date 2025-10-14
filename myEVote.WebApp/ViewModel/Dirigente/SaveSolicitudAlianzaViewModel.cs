using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace myEVote.ViewModel.Dirigente;

public class SaveSolicitudAlianzaViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un partido")]
    [Display(Name = "Partido Político")]
    public int PartidoReceptorId { get; set; }

    public int PartidoSolicitanteId { get; set; }

    public List<SelectListItem> PartidosPoliticos { get; set; } = new();

    public string? ErrorMessage { get; set; }
}