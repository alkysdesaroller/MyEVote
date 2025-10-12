using myEVote.Application.DTOs;
using myEVote.Application.DTOs.Ciudadano;

namespace myEVote.Application.Interfaces.Services;

public interface ICiudadanoService : IGenericService<CiudadanoDto, SaveCiudadanoDto>
{
    Task<CiudadanoDto> GetByCedulaAsync(string cedula);
    Task<bool> ExistsByCedulaAsync(string cedula, int? excludeId = null);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    
}