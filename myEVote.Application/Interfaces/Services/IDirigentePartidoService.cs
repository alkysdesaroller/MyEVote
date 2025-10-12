using myEVote.Application.DTOs.DirigentePolitico;

namespace myEVote.Application.Interfaces.Services;

public interface IDirigentePartidoService
{
    Task<List<DirigentePartidoDto>> GetAllAsync();
    Task<DirigentePartidoDto> GetByUsuarioIdAsync(int usuarioId);
    Task<DirigentePartidoDto> AddAsync(SaveDirigentePartidoDto dto);
    Task DeleteAsync(int id);
    Task<bool> ExistsByUsuarioIdAsync(int usuarioId);
}