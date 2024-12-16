namespace Application.Settings;

public class SmtpSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public bool EnableSsl { get; init; } 
    public string UserName { get; init; }
    public string Password { get; init; }
}