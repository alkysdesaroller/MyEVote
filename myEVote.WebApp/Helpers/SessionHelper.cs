using System.Text.Json;
using myEVote.Application.DTOs.Account;

namespace myEVote.Helpers;

public class SessionHelper
{
    public static void SetUser(ISession session, LoginDto user)
    {
        session.SetString("User", JsonSerializer.Serialize(user));
    }

    public static LoginDto? GetUser(ISession session)
    {
        var userJson = session.GetString("User");
        return userJson == null ? null : JsonSerializer.Deserialize<LoginDto>(userJson);
    }

    public static void ClearSession(ISession session)
    {
        session.Clear();
    }

    public static bool IsAuthenticated(ISession session)
    {
        return session.GetString("User") != null;
    }

    public static bool IsAdmin(ISession session)
    {
        var user = GetUser(session);
        return user?.Rol == "Administrador";
    }

    public static bool IsDirigente(ISession session)
    {
        var user = GetUser(session);
        return user?.Rol == "DirigentePolitico";
    }
}