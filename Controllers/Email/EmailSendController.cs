using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using TASKHIVE.Data;
using MailKit.Net.Smtp;
using TASKHIVE.Models;
using System.Xml.Linq;

namespace TASKHIVE.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSendController : ControllerBase
    {
        public readonly DataContext _context;

        public EmailSendController(DataContext _context)
        {
            this._context = _context;
        }

        [HttpPost]
        public IActionResult SendEmail(int adminId, int PmId, string projectName)
        {

            string adminName;
            User adName = _context.Users
                .Where(x => x.UserId == adminId)
                .FirstOrDefault();

            if (adName != null)
            {
                adminName = adName.Email;
            }
            else
            {
                adminName = null;
            }

            string pManager;
            string receiver;

            User user = _context.Users
                .Where(x => x.UserId == PmId)
                .FirstOrDefault();

            if (user != null)
            {
                receiver = user.Email;
                pManager = user.UserName;
            }
            else
            {
                receiver = null;
                pManager = null;
            }
        
    

            DateTime date = DateTime.Now;

             string body = "<h4>Dear " + pManager +",</h4><p>You have been assigned for a new project. Please check Proxima account for more info.</p><br><p>Project Name: " + projectName + "</p><p>Created Date: " + date + "</p><br><h4>Sincely,<br>" + adminName + "<br>Admin Division</h4>";

             var email = new MimeMessage();
             email.From.Add(MailboxAddress.Parse("surajmshan1234@gmail.com"));
             email.To.Add(MailboxAddress.Parse("surajmshan@gmail.com"));

             email.Subject = "Proxima-New Project Assignment";
             email.Body = new TextPart(TextFormat.Html) { Text = body };

             using var smtp = new SmtpClient();
             smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
             smtp.Authenticate("surajmshan1234@gmail.com", "zkvzwrfytxbjfdra");
             smtp.Send(email);
             smtp.Disconnect(true);

            return Ok();

        }

        [HttpPost("DeveloperAssign")]
        public IActionResult SendEmailtoDev(int[] devId, int projectId)
        {

            string ProjectName;
            Project name = _context.Projects
                .Where(x => x.ProjectId == projectId)
                .FirstOrDefault();

            if (name != null)
            {
                ProjectName = name.ProjectName;
            }
            else
            {
                ProjectName = null;
            }
          
            int x = devId.Length;
            DateTime date = DateTime.Now;

            for (int i = 0; i < x; i++)
            {
                string developer;
                string developerEmail;

                User dev = _context.Users
                    .Where(x => x.UserId == devId[i])
                .FirstOrDefault();

                if (dev != null)
                {
                    developer = dev.UserName;
                    developerEmail = dev.Email;
                }
                else
                {
                    developer = null;
                    developerEmail = null;
                }

                string body = "<h4>Dear " + developer + ",</h4><p>You have been assigned for a new project. Please check Proxima account for more info.</p><br><p>Project Name: " + ProjectName + "</p><p>Created Date: " + date + "</p><br><h4>Admin Division</h4>";

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("surajmshan1234@gmail.com"));
                email.To.Add(MailboxAddress.Parse("surajmshan@gmail.com"));

                email.Subject = "Proxima-New Project Assignment";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate("surajmshan1234@gmail.com", "zkvzwrfytxbjfdra");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            return Ok(new { message = ProjectName });

        }

    }
}
