using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using myEVote.Application.DTOs.Usuario;
using myEVote.Application.Interfaces.Services;
using myEVote.Filters;
using myEVote.ViewModel.Admin;


namespace myEVote.Controllers.Admin;

[Area("Admin")]
[ServiceFilter(typeof(AdminAuthorizacionFilter))]
public class UsuarioController(IUsuarioService service, IEleccionService eleccionService, IMapper mapper)
    : Controller
{
    private readonly IUsuarioService _service = service;
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly IMapper _mapper = mapper;

     public async Task<IActionResult> Index()
        {
            var usuarios = await _service.GetAllAsync();
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            ViewBag.HayEleccionActiva = hayEleccionActiva;
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se puede crear un usuario mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            return View(typeof(SaveUsuarioViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUsuarioViewModel vm)
        {
            // En Create, la contraseña es obligatoria
            if (string.IsNullOrEmpty(vm.Contrasena))
            {
                ModelState.AddModelError("Contrasena", "La contraseña es obligatoria");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
                if (hayEleccionActiva)
                {
                    TempData["Error"] = "No se puede crear un usuario mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                // Validar nombre de usuario único
                var exists = await _service.ExistsByNombreUsuarioAsync(vm.NombreUsuario!);
                if (exists)
                {
                    ModelState.AddModelError("NombreUsuario", "Ya existe un usuario con este nombre");
                    return View(vm);
                }

                var dto = _mapper.Map<SaveUsuarioDto>(vm);
        
                // ✅ Hashear contraseña al crear
                dto.Password = BCrypt.Net.BCrypt.HashPassword(vm.Contrasena);

                await _service.AddAsync(dto);

                TempData["Success"] = "Usuario creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el usuario");
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hayEleccionActiva = await _eleccionService.ExistsEleccionActivaAsync();
            if (hayEleccionActiva)
            {
                TempData["Error"] = "No se puede editar un usuario mientras hay una elección activa";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _service.GetByIdAsync(id);
            var vm = _mapper.Map<SaveUsuarioViewModel>(usuario);
            vm.Contrasena = null; // No mostrar contraseña en edición
            vm.ConfirmarContrasena = null;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUsuarioViewModel vm)
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
                    TempData["Error"] = "No se puede editar un usuario mientras hay una elección activa";
                    return RedirectToAction(nameof(Index));
                }

                // Validar nombre de usuario único
                var exists = await _service.ExistsByNombreUsuarioAsync(vm.NombreUsuario!, vm.Id);
                if (exists)
                {
                    ModelState.AddModelError("NombreUsuario", "Ya existe otro usuario con este nombre");
                    return View(vm);
                }

                var dto = _mapper.Map<SaveUsuarioDto>(vm);
        
                // ✅ Si hay contraseña nueva, hashearla
                if (!string.IsNullOrEmpty(vm.Contrasena))
                {
                    dto.Password= BCrypt.Net.BCrypt.HashPassword(vm.Contrasena); // ✅ Contrasena, no Password
                }

                await _service.UpdateAsync(dto, vm.Id);

                TempData["Success"] = "Usuario actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar el usuario");
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Activate(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmActivate(int id)
        {
            try
            {
                await _service.ActivateAsync(id);
                TempData["Success"] = "Usuario activado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al activar el usuario";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeactivate(int id)
        {
            try
            {
                await _service.DeactivateAsync(id);
                TempData["Success"] = "Usuario desactivado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al desactivar el usuario";
            }

            return RedirectToAction(nameof(Index));
        }
    }

