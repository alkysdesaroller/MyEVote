using AutoMapper;
using myEVote.Application.DTOs.Account;
using myEVote.Application.Interfaces.Repositories;
using myEVote.Application.Interfaces.Services;

namespace myEVote.Application.Services;

public class AccountService(IUsuarioRepository usuarioRepository, IMapper mapper) : IAccountService
{
    public async Task<LoginDto> AuthenticateAsync(string nombreUsuario, string contrasena)
    {
        // Nota: Aquí deberías usar BCrypt para verificar la contraseña hasheada
        // Por ahora lo dejamos simple, pero en producción usa:
        // var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);
        // if (usuario != null && BCrypt.Net.BCrypt.Verify(contrasena, usuario.Contrasena))
            
        var usuario = await usuarioRepository.GetCredentialAsync(nombreUsuario, contrasena);
            
        if (usuario == null!)
            return null!;

        return mapper.Map<LoginDto>(usuario);
    }
}