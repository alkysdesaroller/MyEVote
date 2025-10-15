using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using myEVote.Application.DTOs.Eleccion;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.ViewModel.Admin;

namespace myEVote.Controllers.Admin;


[Area("Admin")]
[ServiceFilter(typeof(AdminAuthorizacionFilter))]
public class EleccionController(
    IEleccionService eleccionService,
    IPuestoElectivoService puestoService,
    IPartidoPoliticoService partidoService,
    ICandidatoPuestoService candidatoPuestoService,
    IMapper mapper)
    : Controller
{
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IPuestoElectivoService _puestoService = puestoService;
    private readonly IPartidoPoliticoService _partidoService = partidoService;
    private readonly ICandidatoPuestoService _candidatoPuestoService = candidatoPuestoService;
    private readonly IMapper _mapper = mapper;

    // GET
        public async Task<IActionResult> Index()
        {
            var elecciones = await _eleccionService.GetAllOrderedAsync();
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            ViewBag.HayEleccionActiva = hayEleccionActiva;
            return View(elecciones);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "Ya existe una elección activa";
                return RedirectToAction(nameof(Index));
            }

            return View(new SaveEleccionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveEleccionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
                if (hayEleccionActiva)
                {
                    TempData["Error"] = "Ya existe una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                // Validar requisitos para crear elección
                var puestosActivos = await _puestoService.GetAllActivosAsync();
                if (!puestosActivos.Any())
                {
                    vm.ErrorMessages.Add("No hay puestos electivos activos. Debe haber al menos un puesto creado y activo.");
                    return View(vm);
                }

                var partidosActivos = await _partidoService.GetAllActivosAsync();
                if (partidosActivos.Count < 2)
                {
                    vm.ErrorMessages.Add("No hay suficientes partidos políticos. Debe haber al menos dos partidos activos.");
                    return View(vm);
                }

                // Validar que cada partido tenga candidatos para TODOS los puestos
                foreach (var partido in partidosActivos)
                {
                    var candidatosPuesto = await _candidatoPuestoService.GetByPartidoPoliticoIdAsync(partido.Id);
                    var puestosSinCandidato = new List<string>();

                    foreach (var puesto in puestosActivos)
                    {
                        var tieneCandidato = candidatosPuesto.Any(cp => cp.PuestoElectivoId == puesto.Id);
                        if (!tieneCandidato)
                        {
                            puestosSinCandidato.Add(puesto.Nombre);
                        }
                    }

                    if (puestosSinCandidato.Any())
                    {
                        vm.ErrorMessages.Add($"El partido político {partido.Nombre} ({partido.Siglas}) no tiene candidatos registrados para los siguientes puestos electivos: {string.Join(", ", puestosSinCandidato)}.");
                    }
                }

                if (vm.HasErrors)
                {
                    return View(vm);
                }

                // Crear la elección
                var dto = _mapper.Map<SaveEleccionDto>(vm);
                await _eleccionService.CreateEleccionAsync(dto);

                TempData["Success"] = "Elección creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                vm.ErrorMessages.Add("Error al crear la elección. Intente nuevamente.");
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Finalizar(int id)
        {
            var eleccion = await _eleccionService.GetEleccionActivaAsync();
            if (eleccion == null! || eleccion.Id != id)
            {
                TempData["Error"] = "Esta elección no está activa";
                return RedirectToAction(nameof(Index));
            }

            return View(eleccion);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmFinalizar(int id)
        {
            try
            {
                await _eleccionService.FinalizarEleccionAsync(id);
                TempData["Success"] = "Elección finalizada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al finalizar la elección";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Resultados(int id)
        {
            var eleccion = await _eleccionService.GetAllOrderedAsync();
            var eleccionSeleccionada = eleccion.FirstOrDefault(e => e.Id == id);

            if (eleccionSeleccionada == null)
            {
                TempData["Error"] = "Elección no encontrada";
                return RedirectToAction(nameof(Index));
            }

            if (eleccionSeleccionada.EstaActiva)
            {
                TempData["Error"] = "No se pueden ver resultados de una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var resultados = await _eleccionService.GetResultadosAsync(id);
            ViewBag.EleccionNombre = eleccionSeleccionada.Nombre;
            ViewBag.EleccionFecha = eleccionSeleccionada.FechaRealizacion;

            return View(resultados);
        }
    }