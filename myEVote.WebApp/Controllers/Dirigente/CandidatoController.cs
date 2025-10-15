using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using myEVote.Application.DTOs.Candidato;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.Helpers;
using myEVote.ViewModel.Dirigente;

namespace myEVote.Controllers.Dirigente;

[Area("Dirigente")]
[ServiceFilter(typeof(DirigenteAuthorizacionFilter))]
public class CandidatoController(
    ICandidatoService service,
    IEleccionService eleccionService,
    IFileUploadService fileUploadService,
    IMapper mapper)
    : Controller
{
    private readonly ICandidatoService _service = service;
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IMapper _mapper = mapper;

    // GET
     public async Task<IActionResult> Index()
        {
            var user = SessionHelper.GetUser(HttpContext.Session);
            var candidatos = await _service.GetByPartidoPoliticoIdAsync(user!.PartidoPoliticoId!.Value);
            
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            ViewBag.HayEleccionActiva = hayEleccionActiva;
            
            return View(candidatos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se puede crear un candidato mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var user = SessionHelper.GetUser(HttpContext.Session);
            var vm = new SaveCandidatoViewModel
            {
                PartidoPoliticoId = user!.PartidoPoliticoId!.Value
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCandidatoViewModel vm)
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
                    TempData["Error"] = "No se puede crear un candidato mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                var user = SessionHelper.GetUser(HttpContext.Session);
                vm.PartidoPoliticoId = user!.PartidoPoliticoId!.Value;

                // Subir foto
                if (vm.Foto != null)
                {
                    vm.FotoUrl = await _fileUploadService.UploadFileAsync(vm.Foto, "fotos-candidatos");
                }

                var dto = _mapper.Map<SaveCandidatoDto>(vm);
                await _service.AddAsync(dto);

                TempData["Success"] = "Candidato creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el candidato");
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se puede editar un candidato mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var candidato = await _service.GetByIdAsync(id);
            var vm = _mapper.Map<SaveCandidatoViewModel>(candidato);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCandidatoViewModel vm)
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
                    TempData["Error"] = "No se puede editar un candidato mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                // Subir nueva foto si se proporcionó
                if (vm.Foto != null)
                {
                    // Eliminar foto anterior
                    if (!string.IsNullOrEmpty(vm.FotoUrl))
                    {
                        _fileUploadService.DeleteFile(vm.FotoUrl);
                    }

                    vm.FotoUrl = await _fileUploadService.UploadFileAsync(vm.Foto, "fotos-candidatos");
                }

                var dto = _mapper.Map<SaveCandidatoDto>(vm);
                await _service.UpdateAsync(dto, vm.Id);

                TempData["Success"] = "Candidato actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el candidato");
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Activate(int id)
        {
            var candidato = await _service.GetByIdAsync(id);
            return View(candidato);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmActivate(int id)
        {
            try
            {
                await _service.ActivateAsync(id);
                TempData["Success"] = "Candidato activado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al activar el candidato";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(int id)
        {
            var candidato = await _service.GetByIdAsync(id);
            return View(candidato);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeactivate(int id)
        {
            try
            {
                await _service.DeactivateAsync(id);
                TempData["Success"] = "Candidato desactivado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al desactivar el candidato";
            }

            return RedirectToAction(nameof(Index));
        }
    }
