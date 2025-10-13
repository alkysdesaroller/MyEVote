using AutoMapper;
using myEVote.Application.DTOs.PartidoPolitico;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class PartidoPoliticoService(IPartidoPoliticoRepository repository, IMapper mapper)
    : GenericService<PartidoPolitico, PartidoPoliticoDto, SavePartidoPoliticoDto>(repository, mapper),
        IPartidoPoliticoService
{
    private readonly IPartidoPoliticoRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<PartidoPoliticoDto>> GetAllActivosAsync()
    {
        var partidos = await _repository.GetAllActivosAsync();
        return _mapper.Map<List<PartidoPoliticoDto>>(partidos);
    }

    public async Task<bool> ExistsBySiglasAsync(string siglas, int? excludeId = null)
    {
        return await _repository.ExistsBySiglasAsync(siglas, excludeId);
    }

    public async Task ActivateAsync(int id)
    {
        var partido = await _repository.GetByIdAsync(id);
        partido.Estado = EstadoEntidad.Activo;
        await _repository.UpdateAsync(partido, id);
    }

    public async Task DeactivateAsync(int id)
    {
        var partido = await _repository.GetByIdAsync(id);
        partido.Estado = EstadoEntidad.Inactivo;
        await _repository.UpdateAsync(partido, id);
    }
}