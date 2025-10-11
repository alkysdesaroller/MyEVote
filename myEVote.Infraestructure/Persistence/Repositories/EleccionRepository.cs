using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class EleccionRepository(MyEVoteContext context) : GenericRepository<Eleccion>(context), IEleccionRepository
{
    private readonly MyEVoteContext _context = context;
    
    public async Task<Eleccion> GetEleccionActivaAsync()
    {
        return await _context.Elecciones
            .FirstOrDefaultAsync(e => e.Estado == EstadoEleccion.EnProceso) ?? throw new InvalidOperationException();
    }

    public async Task<bool> ExistsEleccionActivaAsync()
    {
        return await _context.Elecciones.AnyAsync(e => e.Estado == EstadoEleccion.EnProceso);    }

    public async Task<List<Eleccion>> GetAllOrderedByDateDescAsync()
    {
        return await _context.Elecciones
            .OrderByDescending(e => e.FechaEleccion)
            .ToListAsync();
    }
}