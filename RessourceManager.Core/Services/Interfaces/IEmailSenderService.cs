

using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IEmailSenderService
    {
       Task SendActivationEmailAsync(string email);
       Task SendResetPasswordEmailAsync(string email,string token);
    }
}
