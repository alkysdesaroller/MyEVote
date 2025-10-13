using AutoMapper;
using myEVote.Application.DTOs.AlianzaPolitica;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;

namespace myEVote.Application.Services;

public class AlianzaPoliticaService(IAlianzaPoliticaRepository repository, IMapper mapper) : IAlianzaPoliticaService
{
    public async Task<List<AlianzaPoliticaDto>> GetByPartidoIdAsync(int partidoId)
    {
        var alianzas = await repository.GetByPartidoAsync(partidoId);
        return mapper.Map<List<AlianzaPoliticaDto>>(alianzas);
    }

    public async Task<bool> ExistsAlianzaAsync(int partido1Id, int partido2Id)
    {
        return await repository.ExistsAlianzaAsync(partido1Id, partido2Id);
    }
}