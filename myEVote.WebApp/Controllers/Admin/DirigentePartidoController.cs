using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myEVote.Application.DTOs.DirigentePolitico;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Enums;
using myEVote.Filters;
using myEVote.ViewModel.Admin;

namespace myEVote.Controllers.Admin;

[Area("Admin")]
[ServiceFilter(typeof(AdminAuthorizacionFilter))]
public class DirigentePartidoController(
    IDirigentePartidoService service,
    IUsuarioService usuarioService,
    IPartidoPoliticoService partidoService,
    IEleccionService eleccionService,
    IMapper mapper)
    : Controller
{
    private readonly IDirigentePartidoService _service = service;
    private readonly IUsuarioService _usuarioService = usuarioService;
    private readonly IPartidoPoliticoService _partidoService = partidoService;
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IMapper _mapper = mapper;

    // GET
     public async Task<IActionResult> Index()
        {
            var dirigentes = await _service.GetAllAsync();
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            ViewBag.HayEleccionActiva = hayEleccionActiva;
            return View(dirigentes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se puede crear una asignación mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var vm = new SaveDirigentePartidoViewModel();
            await CargarListasAsync(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveDirigentePartidoViewModel vm)
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
                    TempData["Error"] = "No se puede crear una asignación mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                // Validar que el dirigente no esté ya asignado
                var exists = await _service.ExistsByUsuarioIdAsync(vm.UsuarioId);
                if (exists)
                {
                    ModelState.AddModelError("", "Este dirigente ya está relacionado con otro partido político");
                    await CargarListasAsync(vm);
                    return View(vm);
                }

                var dto = _mapper.Map<SaveDirigentePartidoDto>(vm);
                await _service.AddAsync(dto);

                TempData["Success"] = "Dirigente asignado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al asignar dirigente");
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
                TempData["Error"] = "No se puede eliminar una asignación mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var dirigente = await _service.GetAllAsync();
            var item = dirigente.FirstOrDefault(d => d.Id == id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
                if (hayEleccionActiva)
                {
                    TempData["Error"] = "No se puede eliminar una asignación mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                await _service.DeleteAsync(id);
                TempData["Success"] = "Asignación eliminada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la asignación";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasAsync(SaveDirigentePartidoViewModel vm)
        {
            // Obtener usuarios con rol dirigente y que NO estén ya asignados
            var todosUsuarios = await _usuarioService.GetAllAsync();
            var usuariosDirigentes = todosUsuarios
                .Where(u => u.Rol == RolUsuario.DirigentePolitico && u.Estado == EstadoEntidad.Activo)
                .ToList();

            // Filtrar los que ya tienen asignación
            var dirigentesAsignados = await _service.GetAllAsync();
            var idsAsignados = dirigentesAsignados.Select(d => d.UsuarioId).ToList();

            vm.Usuarios = usuariosDirigentes
                .Where(u => !idsAsignados.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.Nombre} {u.Apellido} ({u.NombreUsuario})"
                })
                .ToList();

            // Partidos activos
            var partidos = await _partidoService.GetAllActivosAsync();
            vm.PartidosPoliticos = partidos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Nombre} ({p.Siglas})"
            }).ToList();
        }
    }
