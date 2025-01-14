using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Tedx.Helper
{
    public static class EmailHelper
    {
        private static IConfiguration _configuration;
        private static IStringLocalizer _localizer;

        public static void Initialize(IConfiguration configuration, IStringLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        public static bool SendEmail(string toEmail, string subject, string body, bool isBodyHtml = true)
        {
            try
            {
                // Get email settings from appsettings.json
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];

                // Configure SMTP client
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true, // Enable SSL/TLS
                };

                // Create the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, // Set to true if the body contains HTML
                };

                // Add the recipient email address
                mailMessage.To.Add(toEmail);

                // Send the email
                smtpClient.Send(mailMessage);

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Log the error (use a proper logger in production)
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email failed to send
            }
        }
    }
}