using myEVote.Application.DTOs.Account;

namespace myEVote.Application.Interfaces.Services;

public interface IAccountService
{
    Task<LoginDto> AuthenticateAsync(string nombreUsuario, string contrasena);
}