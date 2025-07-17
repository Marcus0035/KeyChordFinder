using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailService
{
    private const string SmtpServer = "smtp.gmail.com";
    private const int SmtpPort = 587;
    private const string Username = "email@gmail.com";
    private const string Password = "password"; // App password
    private const string FromName = "KeyChordFinder";

    public async Task SendEmailAsync(string userEmail, string subject, string text, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(FromName, Username));

            message.To.Add(MailboxAddress.Parse(Username));

            message.ReplyTo.Add(MailboxAddress.Parse(userEmail));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = $"Message from user: {userEmail}\n\n{text}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(Username, Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex) { }
    }
}