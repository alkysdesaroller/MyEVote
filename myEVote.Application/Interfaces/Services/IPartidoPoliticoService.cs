using myEVote.Application.DTOs.PartidoPolitico;


namespace myEVote.Application.Interfaces.Services;

public interface IPartidoPoliticoService : IGenericService<PartidoPoliticoDto, SavePartidoPoliticoDto>
{
    Task<List<PartidoPoliticoDto>> GetAllActivosAsync();
    Task<bool> ExistsBySiglasAsync(string siglas, int? excludeId = null);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
}