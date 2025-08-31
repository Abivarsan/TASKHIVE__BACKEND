using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using TASKHIVE.Data;
using MailKit.Net.Smtp;
using TASKHIVE.Models;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using TASKHIVE.Controllers.Email;

namespace TASKHIVE.Controllers.Email
{
    public class EmailService : IEmailService
    {
        public readonly DataContext _context;
        private readonly MailSettings _mailSettings;

        public EmailService(DataContext _context, IOptions<MailSettings> mailSettings)
        {
            this._context = _context;
            _mailSettings = mailSettings.Value;
        }

        public async System.Threading.Tasks.Task SendEmailAsync(int adminId, int PmId, string projectName)
        {
            var admin = _context.Users.FirstOrDefault(x => x.UserId == adminId);
            var pm = _context.Users.FirstOrDefault(x => x.UserId == PmId);

            if (pm == null)
                throw new Exception("Project Manager not found.");

            string body = $@"
        <h4>Dear {pm.UserName},</h4>
        <p>You have been assigned a new project. Please check your TASKHIVE account for details.</p>
        <p>Project Name: {projectName}</p>
        <p>Created Date: {DateTime.Now}</p>
        <br/>
        <h4>Regards,<br>{admin?.Email ?? _mailSettings.SenderName}<br>Admin Division</h4>";

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(pm.Email));
            email.Subject = "TASKHIVE - New Project Assignment";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async System.Threading.Tasks.Task SendEmailtoDevAsync(int[] devIds, int projectId)
        {
            var project = _context.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            if (project == null)
                throw new Exception("Project not found.");

            foreach (var devId in devIds)
            {
                var dev = _context.Users.FirstOrDefault(x => x.UserId == devId);
                if (dev == null) continue;

                string body = $@"<h2>Hello {dev.UserName},</h2>

                    <p>You have been assigned to a new project by {project.ProjectManager} in TASKHIVE. Please review the project details and start planning your tasks accordingly.</p>

                    <hr>

                    <p><strong>Project Name:</strong> {project.ProjectName}</p>
                    <p><strong>Description:</strong> {project.ProjectDescription}</p>
                    <p><strong>Start Date:</strong> {project.P_StartDate}</p>
                    <p><strong>Due Date:</strong> {project.P_DueDate}</p>
                    <p><strong>Priority:</strong> {project.Duration}</p>

                    <hr>

                    <p>Please log in to your TASKHIVE account to check your tasks, deadlines, and resources for this project.</p>

                    <p>Thank you,<br>
                   {project.ProjectManager}<br>
                    TASKHIVE Project Management Team</p>
                    ";

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(dev.Email));
                email.Subject = "TASKHIVE - New Project Assignment";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            
        }

        public async System.Threading.Tasks.Task NotifyClientAsync(int clientId, int projectId)
        {
            // Get client info
            var client = _context.Clients.FirstOrDefault(u => u.ClientId == clientId);
            if (client == null)
                throw new Exception("Client not found.");

            // Get project info
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (project == null)
                throw new Exception("Project not found.");

            // Prepare email body
            string body = $@"<h2>Dear {client.UserName},</h2>

                        <p>We are excited to inform you that your project has officially started! Our team will be working diligently to deliver the best results.</p>

                    <hr>

                    <p><strong>Project Name:</strong> {project.ProjectName}</p>
                    <p><strong>Description:</strong> {project.ProjectDescription}</p>
                    <p><strong>Start Date:</strong> {project.P_StartDate}</p>
                    <p><strong>Expected Completion Date:</strong> {project.P_DueDate}</p>

                    <hr>

                    <p>Our team will keep you updated on the progress. You can also log in to your TASKHIVE client portal for real-time updates and communication with your project manager.</p>

                    <p>Thank you for trusting TASKHIVE with your project.<br>
                    TASKHIVE Team</p>";

            // Create email
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(client.Email));
            email.Subject = $"TASKHIVE - Project '{project.ProjectName}' Started";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // Send email via SMTP
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

            
        }



    }

    public class MailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
