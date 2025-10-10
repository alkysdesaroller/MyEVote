using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IDirigentePartidoRepository : IGenericRepository<DirigentePartido>
{
    Task<DirigentePartido> GetByUsuarioAsync(int usuarioId);
    Task<bool> ExistsByUsuarioIdAsync(int candidatoId);
    
}