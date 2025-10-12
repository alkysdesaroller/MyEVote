using myEVote.Application.DTOs.AlianzaPolitica;

namespace myEVote.Application.Interfaces.Services;

public interface IAlianzaPoliticaService
{
    Task<List<AlianzaPoliticaDto>> GetByPartidoIdAsync(int partidoId);
    Task<bool> ExistsAlianzaAsync(int partido1Id, int partido2Id);
}