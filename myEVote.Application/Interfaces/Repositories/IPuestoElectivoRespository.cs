using myEVote.Application.Interfaces.Repositorios;
using myEVote.Domain.Entities;

namespace myEVote.Application.Interfaces.Repositories;

public interface IPuestoElectivoRespository : IGenericRepository<PuestoElectivo>
{
    Task<List<PuestoElectivo>> GetAllActivosAsync();
}