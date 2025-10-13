using myEVote.Application.DTOs.Eleccion;

namespace myEVote.Application.Interfaces.Services;

public interface IEleccionService
{
    Task<List<EleccionDto>> GetAllOrderedAsync();
    Task<EleccionDto> GetEleccionActivaAsync();
    Task<bool> ExistsEleccionActivaAsync();
    Task<EleccionDto> CreateEleccionAsync(SaveEleccionDto dto);
    Task FinalizarEleccionAsync(int id);
    Task<List<ResultadoPuestoDto>> GetResultadosAsync(int eleccionId);
    Task<ResumenElectoralDto> GetResumenByYearAsync(int anio);
    Task<bool> CanCreateEleccionAsync();
}