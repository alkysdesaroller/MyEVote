using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IPuestoElectivoRepository : IGenericRepository<PuestoElectivo>
{
    Task<List<PuestoElectivo>> GetAllActivosAsync();
}