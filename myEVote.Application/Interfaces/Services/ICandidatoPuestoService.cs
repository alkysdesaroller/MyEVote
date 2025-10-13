using myEVote.Application.DTOs.CandidatoPuesto;

namespace myEVote.Application.Interfaces.Services;

public interface ICandidatoPuestoService 
{
    Task<List<CandidatoPuestoDto>> GetByPartidoPoliticoIdAsync(int partidoPoliticoId);
    Task<CandidatoPuestoDto> AddAsync(SaveCandidatoPuestoDto dto);
    Task DeleteAsync(int id);
    Task<bool> ValidateAsignacionAsync(int candidatoId, int puestoElectivoId, int partidoPoliticoId);
}