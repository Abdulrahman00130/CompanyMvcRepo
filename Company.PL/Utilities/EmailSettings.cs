using System.Net;
using System.Net.Mail;

namespace Company.PL.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("abdo.elkady9999@gmail.com", "wswdujssqwlelirk");
            client.Send("abdo.elkady9999@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
