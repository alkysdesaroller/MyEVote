using AutoMapper;
using myEVote.Application.DTOs.Usuario;
using myEVote.Application.Interfaces.Repositories;

using myEVote.Application.Interfaces.Services;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;

namespace myEVote.Application.Services;

public class UsuarioService(IUsuarioRepository repository, IMapper mapper)
    : GenericService<Usuario, UsuarioDto, SaveUsuarioDto>(repository, mapper), IUsuarioService
{

    public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null)
    {
        return await repository.ExistsByNombreUsuarioAsync(nombreUsuario, excludeId);
    }

    public async Task ActivateAsync(int id)
    {
        var usuario = await repository.GetByIdAsync(id);
        usuario.Estado = EstadoEntidad.Activo;
        await repository.UpdateAsync(usuario, id);
    }

    public async Task DeactivateAsync(int id)
    {
        
        var usuario = await repository.GetByIdAsync(id);
        usuario.Estado = EstadoEntidad.Inactivo;
        await repository.UpdateAsync(usuario, id);
    }
}