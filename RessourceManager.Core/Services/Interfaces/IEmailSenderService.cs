

using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IEmailSenderService
    {
       Task SendEmailAsync(string email, string subject);
    }
}
