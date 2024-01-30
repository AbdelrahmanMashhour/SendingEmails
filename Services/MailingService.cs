
using Microsoft.Extensions.Options;
using MimeKit;
using SendingEmails.Settings;

namespace SendingEmails.Services
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments)
        {
            var email = new MimeMessage
            {
                Sender=MailboxAddress.Parse(_mailSettings.Email)
            };
        }
    }
}
