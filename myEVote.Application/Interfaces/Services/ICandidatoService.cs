using myEVote.Application.DTOs.Candidato;

namespace myEVote.Application.Interfaces.Services;

public interface ICandidatoService : IGenericService<CandidatoDto, SaveCandidatoDto>
{
    Task<List<CandidatoDto>> GetByPartidoPoliticoIdAsync(int partidoPoliticoId);
    Task<List<CandidatoDto>> GetActivosByPartidoPoliticoIdAsync(int partidoPoliticoId);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
}