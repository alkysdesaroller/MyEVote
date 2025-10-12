using AutoMapper;
using myEVote.Application.DTOs;
using myEVote.Application.DTOs.CandidatoPuesto;
using myEVote.Application.DTOs.Ciudadano;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class CiudadanoService : GenericService<Ciudadano, CiudadanoDto, SaveCandidatoPuestoDto>, ICiudadanoService
{
    private readonly ICiudadanoRepository _repository;
    private readonly IMapper _mapper;

    public CiudadanoService(ICiudadanoRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<CiudadanoDto> AddAsync(SaveCiudadanoDto dto)
    {
        var ciudadano = _mapper.Map<Ciudadano>(dto);
        ciudadano.FechaCreacion = DateTime.Now;
        await _repository.AddAsync(ciudadano);
        return _mapper.Map<CiudadanoDto>(ciudadano);
    }

    public async Task UpdateAsync(SaveCiudadanoDto dto, int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<CiudadanoDto> GetByCedulaAsync(string cedula)
    {
        var ciudadano = await _repository.GetByCedulaAsync(cedula);
        return _mapper.Map<CiudadanoDto>(ciudadano);
    }
    

    public async Task<bool> ExistsByCedulaAsync(string cedula, int? excludeId = null)
    {
        return await _repository.ExistsByCedulaAsync(cedula, excludeId);
    }

    public async Task ActivateAsync(int id)
    {
        var ciudadano = await _repository.GetByIdAsync(id);
        ciudadano.Estado = EstadoEntidad.Activo;
        await _repository.UpdateAsync(ciudadano, id);
    }

    public async Task DeactivateAsync(int id)
    {
        var ciudadano = await _repository.GetByIdAsync(id);
        ciudadano.Estado = EstadoEntidad.Inactivo;
        await _repository.UpdateAsync(ciudadano, id);
    }

}
