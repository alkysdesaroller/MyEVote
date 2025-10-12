using myEVote.Application.DTOs.Voto;

namespace myEVote.Application.Interfaces.Services;

public interface IVotoService
{
    Task<bool> HasVotedInEleccionAsync(int ciudadanoId, int eleccionId);
    Task RegistrarVotoAsync(SaveVotoDto dto);
    Task<List<int>> GetPuestosVotadosAsync(int ciudadanoId, int eleccionId);
}