using System.Threading.Tasks;
using EmailSenderTool.Models;

namespace EmailSenderTool.Services
{
    /// <summary>
    /// Represents the service that handles mail messages
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends the email specified by the parameters
        /// </summary>
        /// <param name="options">Informations about the SMTP client and the email to be sent</param>
        Task SendEmailAsync(Options options);
    }
}