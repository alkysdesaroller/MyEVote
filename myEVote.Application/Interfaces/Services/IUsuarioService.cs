using myEVote.Application.DTOs.Usuario;

namespace myEVote.Application.Interfaces.Services;

public interface IUsuarioService : IGenericService<UsuarioDto, SaveUsuarioDto>
{
    Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
}