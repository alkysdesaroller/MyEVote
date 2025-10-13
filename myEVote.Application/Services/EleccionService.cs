using AutoMapper;
using myEVote.Application.DTOs.Eleccion;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class EleccionService(
    IEleccionRepository eleccionRepository,
    IVotoRepository votoRepository,
    IPuestoElectivoRepository puestoRepository,
    IPartidoPoliticoRepository partidoRepository,
    ICandidatoPuestoRepository candidatoPuestoRepository,
    IMapper mapper)
    : IEleccionService
{
    private readonly IEleccionRepository _eleccionRepository = eleccionRepository;
    private readonly IVotoRepository _votoRepository = votoRepository;
    private readonly IPuestoElectivoRepository _puestoRepository = puestoRepository;
    private readonly IPartidoPoliticoRepository _partidoRepository = partidoRepository;
    private readonly ICandidatoPuestoRepository _candidatoPuestoRepository = candidatoPuestoRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<EleccionDto>> GetAllOrderedAsync()
    {
        var elecciones = await _eleccionRepository.GetAllOrderedByDateDescAsync();
        var eleccionesDto = _mapper.Map<List<EleccionDto>>(elecciones);

        // Calcular estadísticas para cada elección
        foreach (var eleccionDto in eleccionesDto)
        {
            var votos = await _votoRepository.GetVotosByEleccionIdAsync(eleccionDto.Id);

            eleccionDto.TotalVotos = votos.Select(v => v.CiudadanoId).Distinct().Count();
            eleccionDto.CantidadPartidos = votos.Select(v => v.Candidato!.PartidoPoliticoId).Distinct().Count();
            eleccionDto.CantidadCandidatos = votos.Select(v => v.CandidatoId).Distinct().Count();
            eleccionDto.CantidadPuestos = votos.Select(v => v.PuestoElectivoId).Distinct().Count();
        }

        return eleccionesDto;
    }

    public async Task<EleccionDto> GetEleccionActivaAsync()
    {
        var eleccion = await _eleccionRepository.GetEleccionActivaAsync();
        return _mapper.Map<EleccionDto>(eleccion);
    }

    public async Task<bool> ExistsEleccionActivaAsync()
    {
        return await _eleccionRepository.ExistsEleccionActivaAsync();
    }

    public async Task<EleccionDto> CreateEleccionAsync(SaveEleccionDto dto)
    {
        var eleccion = _mapper.Map<Eleccion>(dto);
        await _eleccionRepository.AddAsync(eleccion);
        return _mapper.Map<EleccionDto>(eleccion);
    }

    public async Task FinalizarEleccionAsync(int id)
    {
        var eleccion = await _eleccionRepository.GetByIdAsync(id);
        eleccion.Estado = EstadoEleccion.Finalizada;
        await _eleccionRepository.UpdateAsync(eleccion, id);
    }

    public async Task<List<ResultadoPuestoDto>> GetResultadosAsync(int eleccionId)
    {
        var puestos = await _puestoRepository.GetAllActivosAsync();
        var resultados = new List<ResultadoPuestoDto>();

        foreach (var puesto in puestos)
        {
            var votos = await _votoRepository.GetVotosByPuestoAndEleccionAsync(puesto.Id, eleccionId);

            if (votos.Any())
            {
                var totalVotos = votos.Count;

                var candidatosResultado = votos
                    .GroupBy(v => v.CandidatoId)
                    .Select(g => new ResultadoCandidatoDto
                    {
                        CandidatoId = g.Key,
                        CandidatoNombre = g.First().Candidato!.Nombre,
                        CandidatoApellido = g.First().Candidato!.Apellido,
                        CandidatoFotoUrl = g.First().Candidato!.FotoUrl,
                        PartidoPoliticoNombre = g.First().Candidato!.PartidoPolitico!.Nombre,
                        PartidoPoliticoSiglas = g.First().Candidato!.PartidoPolitico!.Siglas,
                        PartidoPoliticoLogoUrl = g.First().Candidato!.PartidoPolitico!.LogoUrl,
                        CantidadVotos = g.Count(),
                        PorcentajeVotos = Math.Round((decimal)g.Count() / totalVotos * 100, 2)
                    })
                    .OrderByDescending(c => c.CantidadVotos)
                    .ToList();

                resultados.Add(new ResultadoPuestoDto
                {
                    PuestoElectivoId = puesto.Id,
                    PuestoElectivoNombre = puesto.Nombre,
                    TotalVotos = totalVotos,
                    Candidatos = candidatosResultado
                });
            }
        }

        return resultados;
    }
    


    public async Task<ResumenElectoralDto> GetResumenByYearAsync(int year)
    {
        var todasElecciones = await _eleccionRepository.GetAllOrderedByDateDescAsync();
        var eleccionesAnio = todasElecciones.Where(e => e.FechaEleccion.Year == year).ToList();
            
        var eleccionesDto = _mapper.Map<List<EleccionDto>>(eleccionesAnio);

        // Calcular estadísticas para cada elección
        foreach (var eleccionDto in eleccionesDto)
        {
            var votos = await _votoRepository.GetVotosByEleccionIdAsync(eleccionDto.Id);
                
            eleccionDto.TotalVotos = votos.Select(v => v.CiudadanoId).Distinct().Count();
            eleccionDto.CantidadPartidos = votos.Select(v => v.Candidato!.PartidoPoliticoId).Distinct().Count();
            eleccionDto.CantidadCandidatos = votos.Select(v => v.CandidatoId).Distinct().Count();
            eleccionDto.CantidadPuestos = votos.Select(v => v.PuestoElectivoId).Distinct().Count();
        }

        return new ResumenElectoralDto
        {
            Year = year,
            Elecciones = eleccionesDto
        };
    }

    public async Task<bool> CanCreateEleccionAsync()
    {
        var existeActiva = await _eleccionRepository.ExistsEleccionActivaAsync();
        if (existeActiva)
            return false;

        
        var puestosActivos = await _puestoRepository.GetAllActivosAsync();
        if (!puestosActivos.Any())
            return false;

       
        var partidosActivos = await _partidoRepository.GetAllActivosAsync();
        if (partidosActivos.Count < 2)
            return false;
        
        foreach (var partido in partidosActivos)
        {
            var candidatosPuesto = await _candidatoPuestoRepository.GetActivosByPartidoIdAsync(partido.Id);
            
            foreach (var puesto in puestosActivos)
            {
                var tieneCandidatoEnPuesto = candidatosPuesto.Any(cp => cp.PuestoElectivoId == puesto.Id);
                if (!tieneCandidatoEnPuesto)
                    return false;
            }
        }

        return true;
    }
}