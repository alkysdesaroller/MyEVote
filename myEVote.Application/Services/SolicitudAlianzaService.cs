using AutoMapper;
using myEVote.Application.DTOs.SolicitudAlianza;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class SolicitudAlianzaService(
    ISolicitudAlianzaRepository repository,
    IAlianzaPoliticaRepository alianzaRepository,
    IMapper mapper)
    : ISolicitudAlianzaService
{
    private readonly ISolicitudAlianzaRepository _repository = repository;
    private readonly IAlianzaPoliticaRepository _alianzaRepository = alianzaRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<SolicitudAlianzaDto>> GetPendientesByPartidoReceptorAsync(int partidoReceptorId)
    {
        var solicitudes = await _repository.GetPendientesByPartidoReceptorAsync(partidoReceptorId);
        return _mapper.Map<List<SolicitudAlianzaDto>>(solicitudes);
    }

    public async Task<List<SolicitudAlianzaDto>> GetByPartidoSolicitanteAsync(int partidoSolicitanteId)
    {
        var solicitudes = await _repository.GetByPartidoSolicitanteAsync(partidoSolicitanteId);
        return _mapper.Map<List<SolicitudAlianzaDto>>(solicitudes);
    }

    public async Task<SolicitudAlianzaDto> AddAsync(SaveSolicitudAlianzaDto dto)
    {
        var solicitud = _mapper.Map<SolicitudAlianza>(dto);
        await _repository.AddAsync(solicitud);
            
        var result = await _repository.GetByIdAsync(solicitud.Id);
        return _mapper.Map<SolicitudAlianzaDto>(result);
    }

    public async Task AceptarAsync(int id)
    {
        var solicitud = await _repository.GetByIdAsync(id);
        solicitud.Estado = EstadoSolicitud.Aceptado;
        await _repository.UpdateAsync(solicitud, id);

        // Crear la alianza política
        var alianza = new AlianzaPolitica
        {
            PartidoPolitico1Id = solicitud.PartidoSolicitanteId,
            PartidoPolitico2Id = solicitud.PartidoReceptorId,
            FechaAlianza = DateTime.Now,
            FechaCreacion = DateTime.Now
        };

        await _alianzaRepository.AddAsync(alianza);
    }

    public async Task RechazarAsync(int id)
    {
        var solicitud = await _repository.GetByIdAsync(id);
        solicitud.Estado = EstadoSolicitud.Rechazado;
        await _repository.UpdateAsync(solicitud, id);
    }

    public async Task DeleteAsync(int id)
    {
        var solicitud = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(solicitud);
    }

    public async Task<bool> CanCreateSolicitudAsync(int partidoSolicitanteId, int partidoReceptorId)
    {
        // Verificar si ya existe una solicitud pendiente entre estos partidos
        var existsPendiente = await _repository.ExistsSolicitudPendienteAsync(partidoSolicitanteId, partidoReceptorId);
            
        if (existsPendiente)
            return false;

        // Verificar si ya existe una alianza entre estos partidos
        var existsAlianza = await _alianzaRepository.ExistsAlianzaAsync(partidoSolicitanteId, partidoReceptorId);
            
        return !existsAlianza;
    }
}