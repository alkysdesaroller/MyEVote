using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using myEVote.Application.Interfaces.Services;

namespace myEVote.Infraestructure.Shared.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var emailMessage = new MimeMessage();

            var fromEmail = _configuration["EmailSetting:FromEmail"] ?? "noreply@myEVote.com";
            var fromName = _configuration["EmailSettings:FromName"] ?? "myEVote";

            emailMessage.From.Add(new MailboxAddress(fromEmail, fromName));

            //envio al usuario
            emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);

            // Autenticación (si está configurada)
            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
            }

            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Log the error (en producción usa un logger real)
            Console.WriteLine($"Error enviando email: {ex.Message}");
            throw;
        }
    }

    public async Task SendVotoConfirmacionAsync(string toEmail, string nombreCiudadano, string nombreEleccion,
        List<string> candidatosVotados)
    {
        var subject = $"Confirmación de Voto - {nombreEleccion}";

        var candidatosHtml = string.Join("", candidatosVotados.Select(c => $"<li>{c}</li>"));

        var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f9f9f9; padding: 20px; margin-top: 20px; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        ul {{ background-color: white; padding: 20px; border-left: 4px solid #007bff; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>eVote360</h1>
                            <h2>Confirmación de Voto</h2>
                        </div>
                        <div class='content'>
                            <h3>Estimado/a {nombreCiudadano},</h3>
                            <p>Su voto ha sido registrado exitosamente en el proceso electoral: <strong>{nombreEleccion}</strong></p>
                            <p>Resumen de sus votos:</p>
                            <ul>
                                {candidatosHtml}
                            </ul>
                            <p>Gracias por ejercer su derecho al voto.</p>
                        </div>
                        <div class='footer'>
                            <p>Este es un correo automático, por favor no responder.</p>
                            <p>&copy; 2025 eVote360. Todos los derechos reservados.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        await SendEmailAsync(toEmail, subject, body);
    }
}