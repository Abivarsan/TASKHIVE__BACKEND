namespace TASKHIVE.Controllers.Email
{
    public interface IEmailService
    {
        Task NotifyClientAsync(int clientId, int projectId);
        Task SendEmailAsync(int adminId, int PmId, string projectName);
        Task SendEmailtoDevAsync(int[] devIds, int projectId);
    }
}
