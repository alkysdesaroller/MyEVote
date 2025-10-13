namespace myEVote.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendVotoConfirmacionAsync(string toEmail, string nombreCiudadano, string nombreEleccion, List<string> candidatosVotados);
}