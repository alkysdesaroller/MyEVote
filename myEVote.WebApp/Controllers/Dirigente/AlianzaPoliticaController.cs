using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myEVote.Application.DTOs.SolicitudAlianza;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.Helpers;
using myEVote.ViewModel.Dirigente;

namespace myEVote.Controllers.Dirigente;

[Area("Dirigente")]
[ServiceFilter(typeof(DirigenteAuthorizacionFilter))]
public class AlianzaPoliticaController(
    IAlianzaPoliticaService alianzaService,
    ISolicitudAlianzaService solicitudService,
    IPartidoPoliticoService partidoService,
    IEleccionService eleccionService,
    IMapper mapper)
    : Controller
{
    private readonly IAlianzaPoliticaService _alianzaService = alianzaService;
    private readonly ISolicitudAlianzaService _solicitudService = solicitudService;
    private readonly IPartidoPoliticoService _partidoService = partidoService;
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IMapper _mapper = mapper;

    public async Task<IActionResult> Index()
        {
            var user = SessionHelper.GetUser(HttpContext.Session);
            var partidoId = user!.PartidoPoliticoId!.Value;

            // Solicitudes pendientes recibidas
            var solicitudesPendientes = await _solicitudService.GetPendientesByPartidoReceptorAsync(partidoId);
            ViewBag.SolicitudesPendientes = solicitudesPendientes;

            // Solicitudes enviadas
            var solicitudesEnviadas = await _solicitudService.GetByPartidoSolicitanteAsync(partidoId);
            ViewBag.SolicitudesEnviadas = solicitudesEnviadas;

            // Alianzas activas
            var alianzas = await _alianzaService.GetByPartidoIdAsync(partidoId);
            ViewBag.Alianzas = alianzas;

            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            ViewBag.HayEleccionActiva = hayEleccionActiva;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateSolicitud()
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se pueden crear solicitudes mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            var vm = new SaveSolicitudAlianzaViewModel
            {
                PartidoSolicitanteId = user!.PartidoPoliticoId!.Value
            };

            await CargarPartidosDisponiblesAsync(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSolicitud(SaveSolicitudAlianzaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarPartidosDisponiblesAsync(vm);
                return View(vm);
            }

            try
            {
                var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
                if (hayEleccionActiva)
                {
                    TempData["Error"] = "No se pueden crear solicitudes mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                var user = SessionHelper.GetUser(HttpContext.Session);
                vm.PartidoSolicitanteId = user!.PartidoPoliticoId!.Value;

                // Validar que se pueda crear la solicitud
                var canCreate = await _solicitudService.CanCreateSolicitudAsync(vm.PartidoSolicitanteId, vm.PartidoReceptorId);
                if (!canCreate)
                {
                    vm.ErrorMessage = "Ya existe una solicitud pendiente o alianza con este partido";
                    await CargarPartidosDisponiblesAsync(vm);
                    return View(vm);
                }

                var dto = _mapper.Map<SaveSolicitudAlianzaDto>(vm);
                await _solicitudService.AddAsync(dto);

                TempData["Success"] = "Solicitud de alianza enviada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = "Error al enviar la solicitud";
                await CargarPartidosDisponiblesAsync(vm);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AceptarSolicitud(int id)
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se pueden aceptar solicitudes mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            var solicitudes = await _solicitudService.GetPendientesByPartidoReceptorAsync(user!.PartidoPoliticoId!.Value);
            var solicitud = solicitudes.FirstOrDefault(s => s.Id == id);

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAceptar(int id)
        {
            try
            {
                await _solicitudService.AceptarAsync(id);
                TempData["Success"] = "Solicitud aceptada. Alianza creada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al aceptar la solicitud";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RechazarSolicitud(int id)
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se pueden rechazar solicitudes mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            var solicitudes = await _solicitudService.GetPendientesByPartidoReceptorAsync(user!.PartidoPoliticoId!.Value);
            var solicitud = solicitudes.FirstOrDefault(s => s.Id == id);

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmRechazar(int id)
        {
            try
            {
                await _solicitudService.RechazarAsync(id);
                TempData["Success"] = "Solicitud rechazada";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al rechazar la solicitud";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se pueden eliminar solicitudes mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            var solicitudes = await _solicitudService.GetByPartidoSolicitanteAsync(user!.PartidoPoliticoId!.Value);
            var solicitud = solicitudes.FirstOrDefault(s => s.Id == id);

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _solicitudService.DeleteAsync(id);
                TempData["Success"] = "Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la solicitud";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarPartidosDisponiblesAsync(SaveSolicitudAlianzaViewModel vm)
        {
            var user = SessionHelper.GetUser(HttpContext.Session);
            var todosPartidos = await _partidoService.GetAllActivosAsync();

            // Filtrar: no incluir el partido del usuario
            // y no incluir partidos con solicitudes pendientes o alianzas existentes
            var partidosDisponibles = new List<SelectListItem>();

            foreach (var partido in todosPartidos.Where(p => p.Id != user!.PartidoPoliticoId!.Value))
            {
                var canCreate = await _solicitudService.CanCreateSolicitudAsync(vm.PartidoSolicitanteId, partido.Id);
                if (canCreate)
                {
                    partidosDisponibles.Add(new SelectListItem
                    {
                        Value = partido.Id.ToString(),
                        Text = $"{partido.Nombre} ({partido.Siglas})"
                    });
                }
            }

            vm.PartidosPoliticos = partidosDisponibles;
        }
    }
