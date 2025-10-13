using AutoMapper;
using myEVote.Application.DTOs.DirigentePolitico;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;

namespace myEVote.Application.Services;

public class DirigentePartidoService(IDirigentePartidoRepository repository, IMapper mapper) : IDirigentePartidoService
{
    
    private readonly IDirigentePartidoRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<DirigentePartidoDto>> GetAllAsync()
    {
        var dirigentes = await _repository.GetAllWithIncludeAsync(new List<string> { "Usuario", "PartidoPolitico" });
        return _mapper.Map<List<DirigentePartidoDto>>(dirigentes);
    }

    public async Task<DirigentePartidoDto> GetByUsuarioIdAsync(int usuarioId)
    {
        var dirigente = await _repository.GetByUsuarioAsync(usuarioId);
        return _mapper.Map<DirigentePartidoDto>(dirigente);
    }

    public async Task<DirigentePartidoDto> AddAsync(SaveDirigentePartidoDto dto)
    {
        var dirigente = _mapper.Map<DirigentePartido>(dto);
        await _repository.AddAsync(dirigente);
            
        var result = await _repository.GetByUsuarioAsync(dto.UsuarioId);
        return _mapper.Map<DirigentePartidoDto>(result);
    }

    public async Task DeleteAsync(int id)
    {
        var dirigente = await _repository.GetByIdAsync(id);
        await _repository.DeleteAsync(dirigente);
    }

    public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
    {
        return await _repository.ExistsByUsuarioIdAsync(usuarioId);
    }
}