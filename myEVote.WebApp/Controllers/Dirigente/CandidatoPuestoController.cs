using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myEVote.Application.DTOs.CandidatoPuesto;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.Helpers;
using myEVote.ViewModel.Dirigente;

namespace myEVote.Controllers.Dirigente;

[Area("Dirigente")]
[ServiceFilter(typeof(DirigenteAuthorizacionFilter))]
public class CandidatoPuestoController(
    ICandidatoPuestoService service,
    ICandidatoService candidatoService,
    IPuestoElectivoService puestoService,
    IAlianzaPoliticaService alianzaService,
    IEleccionService eleccionService,
    IMapper mapper)
    : Controller
{
    private readonly ICandidatoPuestoService _service = service;
    private readonly ICandidatoService _candidatoService = candidatoService;
    private readonly IPuestoElectivoService _puestoService = puestoService;
    private readonly IAlianzaPoliticaService _alianzaService = alianzaService;
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IMapper _mapper = mapper;

    // GET
    public async Task<IActionResult> Index()
    {
        var user = SessionHelper.GetUser(HttpContext.Session);
        var asignaciones = await _service.GetByPartidoPoliticoIdAsync(user!.PartidoPoliticoId!.Value);

        var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
        ViewBag.HayEleccionActiva = hayEleccionActiva;

        return View(asignaciones);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
        if (hayEleccionActiva)
        {
            TempData["Error"] = "No se pueden asignar candidatos mientras hay una elección activa";
            return RedirectToAction(nameof(Index));
        }

        var user = SessionHelper.GetUser(HttpContext.Session);
        var vm = new SaveCandidatoPuestoViewModel
        {
            PartidoPoliticoId = user!.PartidoPoliticoId!.Value
        };

        await CargarListasAsync(vm);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaveCandidatoPuestoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(vm);
            return View(vm);
        }

        try
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se pueden asignar candidatos mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            vm.PartidoPoliticoId = user!.PartidoPoliticoId!.Value;

            // Validar la asignación
            var isValid =
                await _service.ValidateAsignacionAsync(vm.CandidatoId, vm.PuestoElectivoId, vm.PartidoPoliticoId);
            if (!isValid)
            {
                vm.ErrorMessage =
                    "No se puede realizar esta asignación. El candidato ya está asignado a otro puesto en este partido, o está intentando postularse por alianza a un puesto diferente al de su partido original.";
                await CargarListasAsync(vm);
                return View(vm);
            }

            var dto = _mapper.Map<SaveCandidatoPuestoDto>(vm);
            await _service.AddAsync(dto);

            TempData["Success"] = "Candidato asignado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            vm.ErrorMessage = "Error al asignar el candidato";
            await CargarListasAsync(vm);
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
        if (hayEleccionActiva)
        {
            TempData["Error"] = "No se pueden eliminar asignaciones mientras hay una elección activa";
            return RedirectToAction(nameof(Index));
        }

        var user = SessionHelper.GetUser(HttpContext.Session);
        var asignaciones = await _service.GetByPartidoPoliticoIdAsync(user!.PartidoPoliticoId!.Value);
        var asignacion = asignaciones.FirstOrDefault(a => a.Id == id);

        return View(asignacion);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Asignación eliminada exitosamente";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar la asignación";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(SaveCandidatoPuestoViewModel vm)
    {
        var user = SessionHelper.GetUser(HttpContext.Session);

        // Obtener candidatos activos del partido
        var candidatosPartido =
            await _candidatoService.GetActivosByPartidoPoliticoIdAsync(user!.PartidoPoliticoId!.Value);

        // Obtener asignaciones actuales
        var asignacionesActuales = await _service.GetByPartidoPoliticoIdAsync(user.PartidoPoliticoId.Value);
        var idsAsignados = asignacionesActuales.Select(a => a.CandidatoId).ToList();

        // Filtrar candidatos ya asignados
        var candidatosDisponibles = candidatosPartido
            .Where(c => !idsAsignados.Contains(c.Id))
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Nombre} {c.Apellido}"
            })
            .ToList();

        // Obtener alianzas para incluir candidatos de partidos aliados
        var alianzas = await _alianzaService.GetByPartidoIdAsync(user.PartidoPoliticoId.Value);
        foreach (var alianza in alianzas)
        {
            var partidoAliadoId = alianza.PartidoPolitico1Id == user.PartidoPoliticoId.Value
                ? alianza.PartidoPolitico2Id
                : alianza.PartidoPolitico1Id;

            var candidatosAliados = await _candidatoService.GetActivosByPartidoPoliticoIdAsync(partidoAliadoId);

            foreach (var candidato in candidatosAliados)
            {
                candidatosDisponibles.Add(new SelectListItem
                {
                    Value = candidato.Id.ToString(),
                    Text = $"{candidato.Nombre} {candidato.Apellido} ({candidato.PartidoPoliticoSiglas})"
                });
            }
        }

        vm.Candidatos = candidatosDisponibles;

        // Obtener puestos activos
        var puestosActivos = await _puestoService.GetAllActivosAsync();

        // Filtrar puestos ya ocupados por este partido
        var puestosOcupados = asignacionesActuales.Select(a => a.PuestoElectivoId).ToList();

        vm.PuestosElectivos = puestosActivos
            .Where(p => !puestosOcupados.Contains(p.Id))
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Nombre
            })
            .ToList();
    }
}