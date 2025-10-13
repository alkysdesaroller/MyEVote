using AutoMapper;
using myEVote.Application.DTOs.PuestoElectivo;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class PuestoElectivoService(IPuestoElectivoRepository repository, IMapper mapper)
    : GenericService<PuestoElectivo, PuestoElectivoDto, SavePuestoElectivoDto>(repository, mapper),
        IPuestoElectivoService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<PuestoElectivoDto>> GetAllActivosAsync()
    {
        var puestos = await repository.GetAllActivosAsync();
        return _mapper.Map<List<PuestoElectivoDto>>(puestos);
    }

    public async Task ActivateAsync(int id)
    {
        var puesto = await repository.GetByIdAsync(id);
        puesto.Estado = EstadoEntidad.Activo;
        await repository.UpdateAsync(puesto, id);
    }

    public async Task DeactivateAsync(int id)
    { 
        var puesto = await repository.GetByIdAsync(id);
        puesto.Estado = EstadoEntidad.Inactivo;
        await repository.UpdateAsync(puesto, id);
    }
}