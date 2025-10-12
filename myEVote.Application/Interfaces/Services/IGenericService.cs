namespace myEVote.Application.Interfaces.Services;

public interface IGenericService<TDto, in TSaveDto>
where TDto : class
where TSaveDto : class
{
    Task<List<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> AddAsync(TSaveDto dto);
    Task UpdateAsync(TSaveDto dto, int id);
    Task DeleteAsync(int id);
}