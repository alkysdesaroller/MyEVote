using myEVote.Application.DTOs.PuestoElectivo;

namespace myEVote.Application.Interfaces.Services;

public interface IPuestoElectivoService : IGenericService<PuestoElectivoDto, SavePuestoElectivoDto>
{
    Task<List<PuestoElectivoDto>> GetAllActivosAsync();
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    
}