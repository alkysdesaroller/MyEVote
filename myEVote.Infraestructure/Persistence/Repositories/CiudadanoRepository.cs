using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class CiudadanoRepository(MyEVoteContext context) : GenericRepository<Ciudadano>(context), ICiudadanoRepository
{
    private readonly MyEVoteContext _context = context;

    public async Task<Ciudadano> GetByCedulaAsync(string cedula)
    {
        return await _context.Ciudadanos.FirstOrDefaultAsync(c => c.Cedula == cedula) ?? throw new InvalidOperationException();
    }

    public async Task<bool> ExistsByCedulaAsync(string cedula, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Ciudadanos.AnyAsync(c => c.Cedula == cedula && c.Id != excludeId.Value);
        }
        return await _context.Ciudadanos.AnyAsync(c => c.Cedula == cedula);
    }
}