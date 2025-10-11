using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class DirigentePartidoRepository(MyEVoteContext context) : GenericRepository<DirigentePartido>(context), IDirigentePartidoRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<DirigentePartido> GetByUsuarioAsync(int usuarioId)
    {
        return await _context.DirigentePartidos
            .Include(dp => dp.Usuario)
            .Include(dp => dp.PartidoPolitico)
            .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId) ?? throw new InvalidOperationException();
    }

    public async Task<bool> ExistsByUsuarioIdAsync(int usuarioId)
    {
        return await _context.DirigentePartidos.AnyAsync(dp => dp.UsuarioId == usuarioId);
    }
}