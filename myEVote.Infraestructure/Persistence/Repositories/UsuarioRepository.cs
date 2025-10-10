using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class UsuarioRepository(MyEVoteContext context) : GenericRepository<Usuario>(context), IUsuarioRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<Usuario> GetByNombreUsuarioAsync(string nombreUsuario)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario) ?? throw new InvalidOperationException();

    }

    public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario && u.Id != excludeId.Value);
        }
        return await _context.Usuarios.AnyAsync(u => u.NombreUsuario == nombreUsuario);
    }

    public async Task<Usuario> GetCredentialAsync(string nombreUsuario, string password)
    {
        return await _context.Usuarios
            .Include(u => u.DirigentePartido)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Password == password) ?? throw new InvalidOperationException();
    }
}