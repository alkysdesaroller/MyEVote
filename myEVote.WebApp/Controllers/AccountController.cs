using Microsoft.AspNetCore.Mvc;
using myEVote.Application.Interfaces.Services;
using myEVote.Helpers;
using myEVote.ViewModel.Account;

namespace myEVote.Controllers;

public class AccountController(IAccountService accountService) : Controller
{
    private readonly IAccountService _accountService = accountService;
 public IActionResult Login()
        {
            // Si ya está autenticado, redirigir
            if (SessionHelper.IsAuthenticated(HttpContext.Session))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var user = await _accountService.AuthenticateAsync(vm.NombreUsuario, vm.Contrasena);

                if (user == null!)
                {
                    vm.ErrorMessage = "Usuario o contraseña incorrectos";
                    return View(vm);
                }

                // Verificar si es Dirigente sin partido asignado
                if (user.Rol == "DirigentePolitico" && !user.TienePartidoAsignado)
                {
                    vm.ErrorMessage = "No tiene un partido político asignado. Por favor, póngase en contacto con un administrador.";
                    return View(vm);
                }

                // Guardar en sesión
                SessionHelper.SetUser(HttpContext.Session, user);

                // Redirigir según el rol
                if (user.Rol == "Administrador")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (user.Rol == "DirigentePolitico")
                {
                    return RedirectToAction("Index", "Home", new { area = "Dirigente" });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = "Error al iniciar sesión. Intente nuevamente.";
                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            SessionHelper.ClearSession(HttpContext.Session);
            return RedirectToAction("Index", "Home");
        }
    }
