using Microsoft.AspNetCore.Mvc;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.Helpers;
using myEVote.ViewModel.Dirigente;

namespace myEVote.Controllers.Dirigente;

[Area("Dirigente")]
[ServiceFilter(typeof(DirigenteAuthorizacionFilter))]
public class HomeController(
    ICandidatoService candidatoService,
    IAlianzaPoliticaService alianzaService,
    ISolicitudAlianzaService solicitudService,
    ICandidatoPuestoService candidatoPuestoService)
    : Controller
{
    private readonly ICandidatoService _candidatoService = candidatoService;
    private readonly IAlianzaPoliticaService _alianzaService = alianzaService;
    private readonly ISolicitudAlianzaService _solicitudService = solicitudService;
    private readonly ICandidatoPuestoService _candidatoPuestoService = candidatoPuestoService;

    // GET
    public async Task<IActionResult> Index()
    {
        var user = SessionHelper.GetUser(HttpContext.Session);
            
        var vm = new HomeViewModel
        {
            PartidoNombre = user!.PartidoPoliticoNombre,
            PartidoSiglas = user.PartidoPoliticoSiglas,
            PartidoLogoUrl = user.PartidoPoliticoLogoUrl
        };

        // Obtener candidatos del partido
        var candidatos = await _candidatoService.GetByPartidoPoliticoIdAsync(user.PartidoPoliticoId!.Value);
        vm.CantidadCandidatosActivos = candidatos.Count(c => c.Estado == Domain.Enums.EstadoEntidad.Activo);
        vm.CantidadCandidatosInactivos = candidatos.Count(c => c.Estado == Domain.Enums.EstadoEntidad.Inactivo);

        // Obtener alianzas
        var alianzas = await _alianzaService.GetByPartidoIdAsync(user.PartidoPoliticoId.Value);
        vm.CantidadAlianzas = alianzas.Count;

        // Obtener solicitudes pendientes
        var solicitudes = await _solicitudService.GetPendientesByPartidoReceptorAsync(user.PartidoPoliticoId.Value);
        vm.CantidadSolicitudesPendientes = solicitudes.Count;

        // Obtener candidatos asignados
        var asignaciones = await _candidatoPuestoService.GetByPartidoPoliticoIdAsync(user.PartidoPoliticoId.Value);
        vm.CantidadCandidatosAsignados = asignaciones.Count;

        return View(vm);
    }
}
