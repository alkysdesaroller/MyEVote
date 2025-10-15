using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using myEVote.Application.DTOs.Voto;
using myEVote.Application.Interfaces.Services;
using myEVote.ViewModel.Votacion;

namespace myEVote.Controllers;

public class VotacionController(
    IEleccionService eleccionService,
    ICiudadanoService ciudadanoService,
    IVotoService votoService,
    IPuestoElectivoService puestoService,
    ICandidatoService candidatoService,
    ICandidatoPuestoService candidatoPuestoService,
    IOcrService ocrService,
    IEmailService emailService,
    IFileUploadService fileUploadService,
    IMapper mapper)
    : Controller
{
    private readonly IEleccionService _eleccionService = eleccionService;
    private readonly ICiudadanoService _ciudadanoService = ciudadanoService;
    private readonly IVotoService _votoService = votoService;
    private readonly IPuestoElectivoService _puestoService = puestoService;
    private readonly ICandidatoService _candidatoService = candidatoService;
    private readonly ICandidatoPuestoService _candidatoPuestoService = candidatoPuestoService;
    private readonly IOcrService _ocrService = ocrService;
    private readonly IEmailService _emailService = emailService;
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IMapper _mapper = mapper;

    // GET
    [HttpPost]
        public async Task<IActionResult> Verificar(string cedula)
        {
            try
            {
                // Validar que haya una elección activa
                var eleccionActiva = await _eleccionService.GetEleccionActivaAsync();
                if (eleccionActiva == null!)
                {
                    TempData["Error"] = "No hay ningún proceso electoral en estos momentos";
                    return RedirectToAction("Index", "Home");
                }

                // Buscar ciudadano
                var ciudadano = await _ciudadanoService.GetByCedulaAsync(cedula);
                if (ciudadano == null!)
                {
                    TempData["Error"] = "No se encontró un ciudadano con este documento";
                    return RedirectToAction("Index", "Home");
                }

                // Verificar que esté activo
                if (ciudadano.Estado == Domain.Enums.EstadoEntidad.Inactivo)
                {
                    TempData["Error"] = "Este ciudadano está inactivo";
                    return RedirectToAction("Index", "Home");
                }

                // Verificar si ya votó
                var yaVoto = await _votoService.HasVotedInEleccionAsync(ciudadano.Id, eleccionActiva.Id);
                if (yaVoto)
                {
                    TempData["Error"] = "Ya ha ejercido su derecho al voto";
                    return RedirectToAction("Index", "Home");
                }

                // Redirigir a validación de identidad
                return RedirectToAction("ValidarIdentidad", new { cedula });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al verificar el ciudadano";
                return RedirectToAction("Index", "Home");
            }
        }

    [HttpGet]
    public IActionResult ValidarIdentidad(string cedula)
    {
        var vm = new ValidacionIdentidadViewModel
        {
            Cedula = cedula
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ValidarIdentidad(ValidacionIdentidadViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        try
        {
            // Subir foto de cédula
            var cedulaPath = await _fileUploadService.UploadFileAsync(vm.FotoCedula!, "cedulas");
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cedulaPath.TrimStart('/'));

            // Extraer texto con OCR
            var textoExtraido = await _ocrService.ExtractTextFromImageAsync(fullPath);
                
            // Extraer número de cédula
            var cedulaExtraida = _ocrService.ExtractCedulaNumber(textoExtraido);

            // Eliminar archivo temporal
            _fileUploadService.DeleteFile(cedulaPath);

            // Validar que coincidan los números
            if (string.IsNullOrEmpty(cedulaExtraida) || cedulaExtraida != vm.Cedula)
            {
                vm.ErrorMessage = "Los datos extraídos de la foto no coinciden con los datos previamente ingresados. Por favor, intente nuevamente con una foto más clara.";
                return View(vm);
            }

            // Redirigir a selección de puestos
            return RedirectToAction("SeleccionarPuestos", new { documentoIdentidad = vm.Cedula });
        }
        catch (Exception ex)
        {
            vm.ErrorMessage = "Error al procesar la imagen. Asegúrese de que la foto sea clara y esté bien iluminada.";
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> SeleccionarPuestos(string cedula)
    {
        try
        {
            var eleccionActiva = await _eleccionService.GetEleccionActivaAsync();
            var ciudadano = await _ciudadanoService.GetByCedulaAsync(cedula);

            var vm = new ListadoPuestoViewModel
            {
                EleccionId = eleccionActiva.Id,
                EleccionNombre = eleccionActiva.Nombre,
                CiudadanoId = ciudadano.Id
            };

            // Obtener puestos activos
            var puestosActivos = await _puestoService.GetAllActivosAsync();

            // Obtener puestos ya votados
            var puestosVotados = await _votoService.GetPuestosVotadosAsync(ciudadano.Id, eleccionActiva.Id);
            vm.PuestosYaVotados = puestosVotados;

            // Cargar información de puestos
            foreach (var puesto in puestosActivos)
            {
                var candidatosPuesto = await _candidatoPuestoService.GetByPartidoPoliticoIdAsync(0); // Obtener todos
                var candidatosEnPuesto = candidatosPuesto.Where(cp => cp.PuestoElectivoId == puesto.Id).ToList();

                vm.Puestos.Add(new PuestoDisponibleViewModel
                {
                    PuestoId = puesto.Id,
                    PuestoNombre = puesto.Nombre,
                    CantidadPartidos = candidatosEnPuesto.Select(cp => cp.PartidoPoliticoId).Distinct().Count(),
                    CantidadCandidatos = candidatosEnPuesto.Select(cp => cp.CandidatoId).Distinct().Count(),
                    YaVoto = puestosVotados.Contains(puesto.Id)
                });
            }

            return View(vm);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar los puestos disponibles";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> VotarCandidato(int eleccionId, int ciudadanoId, int puestoId)
    {
        try
        {
            var puesto = await _puestoService.GetByIdAsync(puestoId);

            var vm = new VotarCandidatoViewModel
            {
                EleccionId = eleccionId,
                CiudadanoId = ciudadanoId,
                PuestoElectivoId = puestoId,
                PuestoElectivoNombre = puesto.Nombre
            };

            // Obtener todos los candidatos asignados a este puesto
            // Nota: Necesitamos un método en el servicio que obtenga candidatos por puesto
            // Por ahora lo hacemos iterando por todos los partidos
            var candidatosList = new List<CandidatoVotacionViewModel>();

            // Obtener todos los candidatos-puesto para este puesto
            var todasAsignaciones = await _candidatoPuestoService.GetByPartidoPoliticoIdAsync(0);
            var asignacionesPuesto = todasAsignaciones.Where(a => a.PuestoElectivoId == puestoId).ToList();

            foreach (var asignacion in asignacionesPuesto)
            {
                var candidato = await _candidatoService.GetByIdAsync(asignacion.CandidatoId);
                    
                candidatosList.Add(new CandidatoVotacionViewModel
                {
                    CandidatoId = candidato.Id,
                    CandidatoNombre = candidato.Nombre,
                    CandidatoApellido = candidato.Apellido,
                    CandidatoFotoUrl = candidato.FotoUrl,
                    PartidoNombre = asignacion.PartidoPoliticoNombre,
                    PartidoSiglas = asignacion.PartidoPoliticoSiglas,
                    PartidoLogoUrl = candidato.PartidoPoliticoLogoUrl
                });
            }

            // Agregar opción "Ninguno"
            candidatosList.Add(new CandidatoVotacionViewModel
            {
                CandidatoId = 0,
                CandidatoNombre = "Ninguno",
                CandidatoApellido = "",
                CandidatoFotoUrl = "/images/ninguno.png",
                PartidoNombre = "Voto en Blanco",
                PartidoSiglas = "",
                PartidoLogoUrl = ""
            });

            vm.Candidatos = candidatosList;

            return View(vm);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al cargar los candidatos";
            return RedirectToAction("SeleccionarPuestos", new { documentoIdentidad = "" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarVoto(VotarCandidatoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Debe seleccionar un candidato";
            return RedirectToAction("VotarCandidato", new 
            { 
                eleccionId = vm.EleccionId, 
                ciudadanoId = vm.CiudadanoId, 
                puestoId = vm.PuestoElectivoId 
            });
        }

        try
        {
            var dto = new SaveVotoDto
            {
                CiudadanoId = vm.CiudadanoId,
                CandidatoId = vm.CandidatoId,
                PuestoElectivoId = vm.PuestoElectivoId,
                EleccionId = vm.EleccionId
            };

            await _votoService.RegistrarVotoAsync(dto);

            TempData["Success"] = "Voto registrado exitosamente";
                
            // Obtener ciudadano para el redirect
            var ciudadano = await _ciudadanoService.GetByIdAsync(vm.CiudadanoId);
                
            return RedirectToAction("SeleccionarPuestos", new { documentoIdentidad = ciudadano.Cedula });
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al registrar el voto";
            return RedirectToAction("VotarCandidato", new 
            { 
                eleccionId = vm.EleccionId, 
                ciudadanoId = vm.CiudadanoId, 
                puestoId = vm.PuestoElectivoId 
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarVotacion(int eleccionId, int ciudadanoId)
    {
        try
        {
            var eleccion = await _eleccionService.GetEleccionActivaAsync();
            var ciudadano = await _ciudadanoService.GetByIdAsync(ciudadanoId);
            var puestosActivos = await _puestoService.GetAllActivosAsync();
            var puestosVotados = await _votoService.GetPuestosVotadosAsync(ciudadanoId, eleccionId);

            // Verificar que votó por todos los puestos
            if (puestosVotados.Count < puestosActivos.Count)
            {
                var puestosFaltantes = puestosActivos
                    .Where(p => !puestosVotados.Contains(p.Id))
                    .Select(p => p.Nombre)
                    .ToList();

                TempData["Error"] = $"Aún falta votar por los siguientes puestos: {string.Join(", ", puestosFaltantes)}";
                return RedirectToAction("SeleccionarPuestos", new { documentoIdentidad = ciudadano.Cedula });
            }

            // Obtener resumen de votos
            var votos = await _votoService.GetPuestosVotadosAsync(ciudadanoId, eleccionId);
            var candidatosVotados = new List<string>();

            foreach (var puestoId in votos)
            {
                var puesto = await _puestoService.GetByIdAsync(puestoId);
                candidatosVotados.Add($"{puesto.Nombre}: Voto registrado");
            }

            // Enviar correo de confirmación
            await _emailService.SendVotoConfirmacionAsync(
                ciudadano.Email,
                $"{ciudadano.Nombre} {ciudadano.Apellido}",
                eleccion.Nombre,
                candidatosVotados
            );

            TempData["Success"] = "¡Gracias por votar! Se ha enviado una confirmación a su correo electrónico.";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al finalizar la votación";
            var ciudadano = await _ciudadanoService.GetByIdAsync(ciudadanoId);
            return RedirectToAction("SeleccionarPuestos", new { documentoIdentidad = ciudadano.Cedula });
        }
    }
    }
