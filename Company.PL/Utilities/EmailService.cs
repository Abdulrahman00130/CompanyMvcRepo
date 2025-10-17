using Company.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.PL.Utilities
{
    public class EmailService(IOptions<MailSettings> _options) : IEmailService
    {
        //public static void SendEmail(Email email)
        //{
        //    var client = new SmtpClient("smtp.gmail.com", 587);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("abdo.elkady9999@gmail.com", "ifyhshmxepjvlnvb");
        //    client.Send("abdo.elkady9999@gmail.com", email.To, email.Subject, email.Body);

        //}
        public void SendEmail(Email email)
        {
            // building message:
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.Subject = email.Subject;

            // email body
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establishing connection:
            using var client = new SmtpClient();

            // revocation exception
            //client.CheckCertificateRevocation = false;

            client.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_options.Value.Email, _options.Value.Password);

            //Send message
            client.Send(mail);
        }
    }
}
