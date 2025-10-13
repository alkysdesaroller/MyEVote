using AutoMapper;
using myEVote.Application.DTOs.Candidato;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class CandidatoService(
    ICandidatoRepository repository,
    ICandidatoPuestoRepository candidatoPuestoRepository,
    IMapper mapper)
    : GenericService<Candidato, CandidatoDto, SaveCandidatoDto>(repository, mapper), ICandidatoService
{
    private readonly ICandidatoRepository _repository = repository;
    private readonly ICandidatoPuestoRepository _candidatoPuestoRepository = candidatoPuestoRepository;
    private readonly IMapper _mapper = mapper;


    public async Task<List<CandidatoDto>> GetByPartidoPoliticoIdAsync(int partidoPoliticoId)
    {
        var candidatos = await _repository.GetActivosByPartidoPoliticoIdAsync(partidoPoliticoId);
        var candidatosDto = _mapper.Map<List<CandidatoDto>>(candidatos);

        foreach (var candidatoDto in candidatosDto)
        {
            var asignacion = await _candidatoPuestoRepository.GetActivosByPartidoIdAsync(partidoPoliticoId);
            var candidatoAsignacion = asignacion.FirstOrDefault(a => a.CandidatoId == candidatoDto.Id);

            if (candidatoAsignacion != null)
            {
                if (candidatoAsignacion.PuestoElectivo != null)
                    candidatoDto.PuestoElectivoNombre = candidatoAsignacion.PuestoElectivo.Nombre;
            }
            else
            {
                candidatoDto.PuestoElectivoNombre = "Sin puesto asociado";
            }
        }

        return candidatosDto;
    }


    public async Task<List<CandidatoDto>> GetActivosByPartidoPoliticoIdAsync(int partidoPoliticoId)
    {
        var candidatos = await _repository.GetActivosByPartidoPoliticoIdAsync(partidoPoliticoId);
        return _mapper.Map<List<CandidatoDto>>(candidatos);
    }

    public async Task ActivateAsync(int id)
    {
        var candidato = await _repository.GetByIdAsync(id);
        candidato.Estado = EstadoEntidad.Activo;
        await _repository.UpdateAsync(candidato, id);
    }

    public async Task DeactivateAsync(int id)
    {
        var candidato = await _repository.GetByIdAsync(id);
        candidato.Estado = EstadoEntidad.Inactivo;
        await _repository.UpdateAsync(candidato, id);
    }
}