
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendingEmails.Settings;
//using System.Net.Mail;
using MailKit.Net.Smtp;//     
using System.Threading.Tasks;

namespace SendingEmails.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        /*
         * Send to
         * subject
         * Body+Attachments
         */

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),//Sender
                Subject = subject//subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));//Resever ---→ sendTo

            var builder = new BodyBuilder();//To contain all attachments
            if (attachments is not null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms= new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes=ms.ToArray();
                        builder.Attachments.Add(file.FileName, fileBytes,   ContentType.Parse(file.ContentType));
                    }
                }

            }
            builder.HtmlBody = body;//→Body
            email.Body = builder.ToMessageBody();

            email.From.Add(new MailboxAddress(_mailSettings.DesplayName, _mailSettings.Email));
            
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);



        }
    }
}
