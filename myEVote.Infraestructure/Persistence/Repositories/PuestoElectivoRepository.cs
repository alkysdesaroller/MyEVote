using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class PuestoElectivoRepository(MyEVoteContext context) : GenericRepository<PuestoElectivo>(context), IPuestoElectivoRespository
{
    private readonly MyEVoteContext _context = context;

    public async Task<List<PuestoElectivo>> GetAllActivosAsync()
    {
        return await _context.PuestosElectivos
            .Where(p => p.Estado == EstadoEntidad.Activo)
            .ToListAsync();
    }
}