using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendingEmails.DTOs;
using SendingEmails.Services;

namespace SendingEmails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly IMailingService _mailingService;
        public MailingController(IMailingService mailingService)
        {
            _mailingService=mailingService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequestDto dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok();


        }
        [HttpPost("welcom")]
        public async Task<IActionResult> WelcomRequest(WelcomRequestDto dto)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\Template\\EmailTemplate.html";
            var str = new StreamReader(path);
            var body = str.ReadToEnd();
            str.Close();
            body = body.Replace("[username]", dto.UserName).Replace("[email]", dto.Email);
            _mailingService.SendEmailAsync(dto.Email, "Testing Template Using Html Page", body);
            return Ok("Now go and check your Gmail");
        }

    }
}
