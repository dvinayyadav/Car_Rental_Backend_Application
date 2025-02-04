using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Car_Rental_Backend_Application.Data
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // ✅ Gmail SMTP server
        private readonly int _smtpPort = 587;                   // ✅ Use 587 for TLS
        private readonly string _smtpUsername = "devaravinay698@gmail.com"; // ✅ Your Gmail
        private readonly string _smtpPassword = "cmjrfysflvsqfkfu";  // ✅ Use the new app password


        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            // Set the sender
            emailMessage.From.Add(new MailboxAddress("Car Rental Service", _smtpUsername));

            // ✅ Corrected recipient email format
            emailMessage.To.Add(new MailboxAddress(toEmail, toEmail));

            // Set the subject
            emailMessage.Subject = subject;

            // Set the message body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message // HTML body for rich email formatting
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // Send the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
