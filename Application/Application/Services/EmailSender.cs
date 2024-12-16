using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using Application.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EmailSender(IOptions<SmtpSettings> smtpSettings, ILogger<EmailSender> logger) : IEmailSender<User>
{
    private SmtpSettings _smtpSettings { get; set; } = smtpSettings.Value;

    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {// Example implementation
        var message = new MailMessage();
        message.To.Add(email);
        message.From = new MailAddress("let@let.com");
        message.Subject = "Confirmation Link";
        message.Body = $"Hello {user.UserName}, please confirm your email by clicking on the following link: {confirmationLink}";
        message.IsBodyHtml = true;

        using (var smtpClient = new SmtpClient("smtp.example.com"))
        {
            //await smtpClient.SendMailAsync(message);
            //Log instead of sending email
            logger.LogInformation($"Email sent to {email} with confirmation link {confirmationLink}");
        }
    }
    private SmtpClient buildClient()
    {
        return new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };
    }   

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {

        SmtpClient client = buildClient();
        //Loggin instead of sending email
        logger.LogInformation($"Email sent to {email} with subject {subject} and message \n{htmlMessage}\n");
        //return client.SendMailAsync(
        //    new MailMessage("no-reply@yourdomain.com", email, subject, htmlMessage)
        //    {
        //        IsBodyHtml = true
        //    });
        return Task.CompletedTask;
    }
 
    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        string subject = "Réinitialisation de votre mot de passe";
        string htmlMessage = $"<p>Bonjour {user.UserName},</p><p>Votre code de réinitialisation de mot de passe est : <strong>{resetCode}</strong></p>";

        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        string subject = "Réinitialisation de votre mot de passe";
        string htmlMessage = $"<p>Bonjour {user.UserName},</p><p>Votre lien pour réinitialiser votre mot de passe est : <a href=\"{resetLink}\"><strong>{resetLink}</strong></p></a>";

        await SendEmailAsync(email, subject, htmlMessage);
    }
}
