using AutoMapper;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Application.Interfaces.Services;

namespace myEVote.Application.Services;

public class GenericService<TEntity, TDto,TSaveDto> : IGenericService<TDto,TSaveDto>
    where TEntity : class
    where TDto : class
    where TSaveDto : class
{
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TDto> AddAsync(TSaveDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(entity);
    }

    public virtual async Task<List<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<List<TDto>>(entities);
    }

    public virtual async Task<TDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task UpdateAsync(TSaveDto dto, int id)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.UpdateAsync(entity, id);
    }
}