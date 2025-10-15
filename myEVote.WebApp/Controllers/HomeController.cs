using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myEVote.Helpers;
using myEVote.Models;

namespace myEVote.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Si ya está autenticado como admin o dirigente, redirigir
        if (SessionHelper.IsAdmin(HttpContext.Session))
        {
            return RedirectToAction("Index", "Home", new { area = $"Admin" });
        }
            
        if (SessionHelper.IsDirigente(HttpContext.Session))
        {
            return RedirectToAction("Index", "Home", new { area = $"Dirigente" });
        }

        // Mostrar página de votación
        return View();
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}