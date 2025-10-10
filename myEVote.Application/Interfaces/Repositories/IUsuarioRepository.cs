using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario> GetByNombreUsuarioAsync(string nombreUsuario);
    Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null);
    Task<Usuario> GetCredentialAsync(string nombreUsuario, string password);
}