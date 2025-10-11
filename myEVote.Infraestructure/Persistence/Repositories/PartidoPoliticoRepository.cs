using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class PartidoPoliticoRepository(MyEVoteContext context) : GenericRepository<PartidoPolitico>(context), IPartidoPoliticoRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<List<PartidoPolitico>> GetAllActivosAsync()
    {
        return await _context.PartidosPoliticos
            .Where(p => p.Estado == EstadoEntidad.Activo)
            .ToListAsync();
    }

    public async Task<bool> ExistsBySiglasAsync(string siglas, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.PartidosPoliticos.AnyAsync(p => p.Siglas == siglas && p.Id != excludeId.Value);
        }
        return await _context.PartidosPoliticos.AnyAsync(p => p.Siglas == siglas);
    }
}