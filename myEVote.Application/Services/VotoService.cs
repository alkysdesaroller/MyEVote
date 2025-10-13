using AutoMapper;
using myEVote.Application.DTOs.Voto;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;

namespace myEVote.Application.Services;

public class VotoService(IVotoRepository repository, IMapper mapper) : IVotoService
{
    private readonly IVotoRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> HasVotedInEleccionAsync(int ciudadanoId, int eleccionId)
    {
        return await _repository.HasVotedInEleccionAsync(ciudadanoId, eleccionId);
    }

    public async Task RegistrarVotoAsync(SaveVotoDto dto)
    {
        var voto = _mapper.Map<Voto>(dto);
        await _repository.AddAsync(voto);
    }

    public async Task<List<int>> GetPuestosVotadosAsync(int ciudadanoId, int eleccionId)
    {
       
        var votos = await _repository.GetVotosByEleccionIdAsync(eleccionId);
        return votos
            .Where(v => v.CiudadanoId == ciudadanoId)
            .Select(v => v.PuestoElectivoId)
            .Distinct()
            .ToList();
    }
}