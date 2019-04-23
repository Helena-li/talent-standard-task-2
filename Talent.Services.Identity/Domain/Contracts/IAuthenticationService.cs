using Talent.Common.Auth;
using Talent.Common.Models;
using System.Threading.Tasks;
using Talent.Common.Commands;
using Talent.Services.Identity.Domain.Models;
using Talent.Services.Identity.Domain.Models.Client;

namespace Talent.Services.Identity.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<JsonWebToken> SignUp(CreateUser user);
        Task<bool> UniqueEmail(string email);
        Task<bool> VerifyPassword(string email, string password);
       
        Task<JsonWebToken> LoginAsync(string email, string password);
      
        Task<bool> IsEmailVerified(string email, string userRole);

        Task<bool> VerifyToken(string email, string token);
        Task<string> GetAccountSetting(string userId, string userRole);
        Task<string> ChangeUserName(string userId, string userRole, string userName);
        Task<ChangePasswordResult> ChangePassword(string userId, string userRole, ChangePasswordViewModel model);
        Task<bool> DeactivateAccount(string userId, string userRole);

        //recruiters
        Task<string> AddEmployer(UpdateClientViewModal result);
        Task<Client> AddEmployerAsClientAsync(string employerId, string recruiterId);
        
        Task RemoveClientFromList(string clientId, string recruiterID);
        Task<bool> ConfirmEmailForClient(ClientInvitationViewModal command);
        Task<Employer> ResetEmployerPassword(string email);
        Task UpdateClientStatus(string recruiterId, string clientId, InvitationStatus status);
 

        //helper meathod for recruiter/client
        Task<bool> IsRecruiterAuthorised(string clientId, string recruiterId);
        Task<Recruiter> GetRecruiterFromEmail(string email);
        Task<Employer> GetEmployerFromEmail(string email);
    }
}
