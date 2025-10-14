using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace myEVote.ViewModel.Dirigente;

public class SaveCandidatoPuestoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un candidato")]
    [Display(Name = "Candidato")]
    public int CandidatoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un puesto electivo")]
    [Display(Name = "Puesto Electivo")]
    public int PuestoElectivoId { get; set; }

    public int PartidoPoliticoId { get; set; }

    public List<SelectListItem> Candidatos { get; set; } = new();
    public List<SelectListItem> PuestosElectivos { get; set; } = new();

    public string? ErrorMessage { get; set; }
}