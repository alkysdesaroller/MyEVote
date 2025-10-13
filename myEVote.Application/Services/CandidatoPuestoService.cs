using AutoMapper;
using myEVote.Application.DTOs.CandidatoPuesto;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;

namespace myEVote.Application.Services;

public class CandidatoPuestoService(
    ICandidatoPuestoRepository repository,
    ICandidatoRepository candidatoRepository,
    IMapper mapper)
    : ICandidatoPuestoService
{
    private readonly ICandidatoPuestoRepository _repository = repository;
    private readonly ICandidatoRepository _candidatoRepository = candidatoRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<CandidatoPuestoDto>> GetByPartidoPoliticoIdAsync(int partidoPoliticoId)
    {
        var candidatoPuestos = await _repository.GetActivosByPartidoIdAsync(partidoPoliticoId);
        return _mapper.Map<List<CandidatoPuestoDto>>(candidatoPuestos);
    }

    public async Task<CandidatoPuestoDto> AddAsync(SaveCandidatoPuestoDto dto)
    {
        var candidatoPuesto = _mapper.Map<CandidatoPuesto>(dto);
        await _repository.AddAsync(candidatoPuesto);
            
        var result = await _repository.GetByCandidatoPuestoAsync(dto.CandidatoId, dto.PuestoElectivoId);
        return _mapper.Map<CandidatoPuestoDto>(result);
    }

    public async Task DeleteAsync(int id)
    {
      
        var candidatoPuesto = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(candidatoPuesto);
    }

    public async Task<bool> ValidateAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoPoliticoId)
    {
        var exists  = await _repository.ExistsAsignacionAsync(candidatoId, puestoElectivoId, partidoPoliticoId);
        if(exists) return false;
        
        var candidato  = await _candidatoRepository.GetByIdAsync(candidatoId);
        if (candidato.PartidoPoliticoId == puestoElectivoId)
        {
            var asignaciones = await _repository.GetActivosByPartidoIdAsync(partidoPoliticoId);
            var yaAsignado = asignaciones.Any(a => a.CandidatoId == candidatoId);
            
            return !yaAsignado;
        }
        else
        {
            var asignacionOriginal =  await _repository.GetByCandidatoPuestoAsync(candidatoId, puestoElectivoId);

            if (asignacionOriginal != null && asignacionOriginal.PartidoPoliticoId == candidato.PartidoPoliticoId)
            {
                return true;
            }

            return false;
        }
    }
}