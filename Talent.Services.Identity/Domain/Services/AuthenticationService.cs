using Talent.Common.Auth;
using Talent.Common.Contracts;
using Talent.Common.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Commands;
using System.Text;
using System.Web;
using Talent.Services.Identity.Domain.Models;
using Talent.Services.Identity.Domain.Models.Client;

namespace Talent.Services.Identity.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;

        private IRepository<User> _userRepository;
        private IRepository<Employer> _employerRepository;
        private IRepository<Login> _loginRepository;
        private IRepository<Recruiter> _recruitorRepository;
        private IPasswordStorage _encryptPassword;
        private IJwtHandler _jwtHandler;




        public AuthenticationService(IRepository<User> userRepository,
                                IRepository<Employer> employerRepository,
                                IRepository<Login> loginRepository,
                                IRepository<Recruiter> recruitorRepository,
                                IPasswordStorage encryptPassword,
                                IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _loginRepository = loginRepository;
            _recruitorRepository = recruitorRepository;
            _encryptPassword = encryptPassword;
            _jwtHandler = jwtHandler;
   
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        public async Task<JsonWebToken> SignUp(CreateUser user)
        {
            try
            {
                if (user == null) throw new ApplicationException("Incomplete register request - user is null");
                if (user.Email == null) throw new ApplicationException("Incomplete register request - user's email is null");
                if (user.Password == null || user.Password.Length == 0) throw new ApplicationException("Incomplete register request - Password is null");

                // hash password
                var passHash = _encryptPassword.CreateHash(user.Password);

                //var passHash = new PBKDF2(user.Password,SALT_BYTES,PBKDF2_ITERATIONS,"HMACSHA512");
                var UId = Guid.NewGuid();
                var objectId = ObjectId.GenerateNewId().ToString();
                var login = new Login()
                {
                    Id = objectId,
                    UId = UId,
                    Username = user.Email,
                    PasswordHash = passHash,
                    IsDisabled = false,
                    EmailAddressAuthorized = true,
                    ExpiredOn = DateTime.UtcNow.AddHours(24),
                    PasswordFormat = PBKDF2_ITERATIONS,
                    TermsAccepted = user.TermsConditionsAccepted
                };

                if (user.UserRole == "recruiter")
                {
                    var newRecruitor = new Recruiter()
                    {
                        Id = objectId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        UId = UId,
                        Login = login
                    };
                    newRecruitor.CompanyContact.Email = user.Email;
                    newRecruitor.CompanyContact.Name = user.CompanyName;

                    await _recruitorRepository.Add(newRecruitor);
                    return _jwtHandler.Create(newRecruitor.Id, user.UserRole, true);
                }
                else if (user.UserRole == "employer")
                {
                    var newEmployer = new Employer()
                    {
                        Id = objectId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        UId = UId,
                        Login = login,
                    };
                    newEmployer.CompanyContact.Email = user.Email;
                    newEmployer.CompanyContact.Name = user.CompanyName;

                    await _employerRepository.Add(newEmployer);
                    return _jwtHandler.Create(newEmployer.Id, user.UserRole, true);
                }
                else
                {
                    var newTalent = new User()
                    {
                        Id = objectId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false,
                        UId = UId,
                        Login = login,
                    };

                    await _userRepository.Add(newTalent);
                    return _jwtHandler.Create(newTalent.Id, user.UserRole, true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Register error - " + ex.Message);
            }
        }

        /// <summary>
        /// Verify password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> VerifyPassword(string email, string password)
        {
            var talent = (await _userRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
            if (talent != null)
            {
                return await VerifyPassword(talent.Login, password);
            }
            var employer = (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
            if (employer != null)
            {
                return await (VerifyPassword(employer.Login, password));
            }
            return false;
        }

        public async Task<bool> VerifyPassword(Login login, string password)
        {
            //TODO for testing purpose
            if (string.IsNullOrEmpty(password)) throw new ApplicationException("Login fail - Password is null");

            if (login == null) throw new ApplicationException("Login fail - user is null");

            return _encryptPassword.VerifyPassword(password, login.PasswordHash);
        }

        public async Task ResetPassword(Login user, string newPassword)
        {
            if (user == null) throw new ApplicationException("Incomplete reset password - user is null");
            // hash password
            var passHash = _encryptPassword.CreateHash(newPassword);
            user.PasswordHash = passHash;

            await _loginRepository.Update(user);

        }

        

       

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var talentUser = (await _userRepository.Get(x => x.Login.Username == email)).FirstOrDefault();

            string userRole = "talent";
            var login = new Login();
            var employerUser = new Employer();
            var recruiterUser = new Recruiter();

            if (talentUser != null)
            {
                login = talentUser.Login;
                //reactivate account if the account is currently deactivated
                if (talentUser.IsDeleted)
                {
                    talentUser.IsDeleted = false;
                    await _userRepository.Update(talentUser);
                }
            }
            else if (talentUser == null)
            {
                employerUser = (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
                if (employerUser != null)
                {
                    userRole = "employer";
                    login = employerUser.Login;
                    //reactivate account if the account is deactivated
                    if (employerUser.IsDeleted)
                    {
                        employerUser.IsDeleted = false;
                        await _employerRepository.Update(employerUser);
                    }
                }

            }
            if (talentUser == null && employerUser == null)
            {
                recruiterUser = (await _recruitorRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
                if (recruiterUser != null)
                {
                    userRole = "recruiter";
                    login = recruiterUser.Login;
                    //reactivate account if the account is deactivated
                    if (recruiterUser.IsDeleted)
                    {
                        recruiterUser.IsDeleted = false;
                        await _recruitorRepository.Update(recruiterUser);
                    }
                }
            }
            else if (talentUser == null && employerUser == null && recruiterUser == null)
            {
                throw new ApplicationException("Invalid credentials");
            }

            var passwordCorrect = await VerifyPassword(login, password);

            if (!passwordCorrect)
            {
                throw new ApplicationException("Invalid credentials");
            }

            return _jwtHandler.Create(login.Id, userRole, false);
        }

        public async Task<bool> IsEmailVerified(string email, string userRole)
        {
            bool isVerified;

            switch (userRole.ToLower())
            {
                case "talent":
                    var existingTalent = (await _userRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
                    isVerified = existingTalent.Login.EmailAddressAuthorized;
                    return isVerified;
                case "recruiter":
                    var recruiter = (await _recruitorRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
                    isVerified = recruiter.Login.EmailAddressAuthorized;
                    return isVerified;
                case "employer":
                    var existingEmployer = (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
                    isVerified = existingEmployer.Login.EmailAddressAuthorized;
                    return isVerified;
                default:
                    throw new ApplicationException("Invalid email");
            }
        }

      

        
        public async Task<bool> UniqueEmail(string email)
        {
            var existingTalent = (await _userRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
            var existingEmployer = (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
            var existingRecruiter = (await _recruitorRepository.Get(x => x.Login.Username == email)).FirstOrDefault();

            if (existingTalent == null && existingEmployer == null && existingRecruiter == null)
            {
                return true;
            }

            return false;
        }
        public async Task<UserRole> DetermineUserRole(string email)
        {
            var talentUser = (await _userRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
            var employerUser = new Employer();
            var recruiterUser = new Recruiter();

            if (talentUser == null)
            {
                employerUser = (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();

                if (employerUser == null)
                {
                    recruiterUser = (await _recruitorRepository.Get(x => x.Login.Username == email)).FirstOrDefault();

                    if (recruiterUser == null)
                    {
                        throw new ApplicationException("Invalid Email");
                    }
                    else
                    {
                        return (UserRole.Recruiter);
                    }
                }
                else
                {
                    return (UserRole.Employer);
                }
            }
            else
            {
                return (UserRole.Talent);
            }
        }
       


        public async Task<string> GeneratePasswordResetTokenAsync(string id)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(id);

            result.Append(characters[random.Next(characters.Length)]);

            return result.ToString();
        }

 

        public async Task<bool> VerifyToken(string email, string token)
        {
            try
            {
                UserRole userRole = await DetermineUserRole(email);
                bool isTokenValid;
                if (userRole == UserRole.Talent)
                {
                    var user = _userRepository.GetQueryable()
                        .Where(x => x.Login.ResetPasswordToken.Equals(token)
                        && x.Login.Username.ToLowerInvariant() == email.ToLowerInvariant()
                        && x.Login.ResetPasswordTokenExpiryDate >= DateTime.Today
                        )
                        .FirstOrDefault();
                    isTokenValid = (user != null && !(string.IsNullOrEmpty(user.Id))) ? true : false;
                }
                else if (userRole == UserRole.Employer)
                {
                    var user = _employerRepository.GetQueryable()
                        .Where(x => x.Login.ResetPasswordToken.Equals(token)
                        && x.Login.Username.ToLowerInvariant() == email.ToLowerInvariant()
                        && x.Login.ResetPasswordTokenExpiryDate >= DateTime.Today
                        )
                        .FirstOrDefault();
                    isTokenValid = (user != null && !(string.IsNullOrEmpty(user.Id))) ? true : false;
                }
                else if (userRole == UserRole.Recruiter)
                {
                    var user = _recruitorRepository.GetQueryable()
                        .Where(x => x.Login.ResetPasswordToken.Equals(token)
                        && x.Login.Username.ToLowerInvariant() == email.ToLowerInvariant()
                        && x.Login.ResetPasswordTokenExpiryDate >= DateTime.Today
                        )
                        .FirstOrDefault();
                    isTokenValid = (user != null && !(string.IsNullOrEmpty(user.Id))) ? true : false;
                }
                else
                {
                    throw new ApplicationException("Token Invalid");
                }

                return isTokenValid;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error");
            }
        }

        public async Task<string> GetAccountSetting(string userId, string userRole)
        {
            //TODO: when notifications are implemented and user is able to save notification/account settings
            //this method will be used to retrieve the settings, for now it will only get user's name/company name

            if (userRole == "talent")
            {
                var user = (await _userRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                return (user.FirstName + "," + user.LastName);
            }
            else if (userRole == "employer")
            {

                var user = (await _employerRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                return (user.CompanyContact.Name);
            }
            else if (userRole == "recruiter")
            {

                var user = (await _recruitorRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                return (user.CompanyContact.Name);
            }
            else
            {
                throw new ApplicationException("User not found");
            }

        }

        public async Task<string> ChangeUserName(string userId, string userRole, string userName)
        {
            if (userRole == "talent")
            {
                var user = (await _userRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                user.FirstName = userName.Split(',')[0];
                user.LastName = userName.Split(',')[1];
                await _userRepository.Update(user);
                return user.FirstName + "," + user.LastName;
            }
            else if (userRole == "employer")
            {

                var user = (await _employerRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                user.CompanyContact.Name = userName;
                await _employerRepository.Update(user);

                return (user.CompanyContact.Name);
            }
            else if (userRole == "recruiter")
            {

                var user = (await _recruitorRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                user.CompanyContact.Name = userName;
                await _recruitorRepository.Update(user);
                return (user.CompanyContact.Name);
            }
            else
            {
                throw new ApplicationException("User not found");
            }
        }
        public async Task<ChangePasswordResult> ChangePassword(string userId, string userRole, ChangePasswordViewModel model)
        {
            if (!(model.NewPassword == model.ConfirmPassword)) return (new ChangePasswordResult() { Success = false, Message = "New password and confirm password don't match" });
            if (userRole == "talent")
            {
                var user = (await _userRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                var passwordCorrect = await VerifyPassword(user.Login, model.CurrentPassword);
                if (!passwordCorrect)
                {
                    return (new ChangePasswordResult() { Success = false, Message = "Current password is incorrect" });
                }
                var passHash = _encryptPassword.CreateHash(model.NewPassword);
                user.Login.PasswordHash = passHash;
                await _userRepository.Update(user);
                return (new ChangePasswordResult() { Success = true, Message = "Successfully changed password" });
            }
            else if (userRole == "employer")
            {
                var user = (await _employerRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                var passwordCorrect = await VerifyPassword(user.Login, model.CurrentPassword);
                if (!passwordCorrect)
                {
                    return (new ChangePasswordResult() { Success = false, Message = "Current password is incorrect" });
                }
                var passHash = _encryptPassword.CreateHash(model.NewPassword);
                user.Login.PasswordHash = passHash;
                await _employerRepository.Update(user);
                return (new ChangePasswordResult() { Success = true, Message = "Successfully changed password" });
            }
            else if (userRole == "recruiter")
            {
                var user = (await _recruitorRepository.Get(x => x.Id == userId)).FirstOrDefault();
                if (user == null)
                {
                    throw new ApplicationException("User not found");
                }
                var passwordCorrect = await VerifyPassword(user.Login, model.CurrentPassword);
                if (!passwordCorrect)
                {
                    return (new ChangePasswordResult() { Success = false, Message = "Current password is incorrect" });
                }
                var passHash = _encryptPassword.CreateHash(model.NewPassword);
                user.Login.PasswordHash = passHash;
                await _recruitorRepository.Update(user);
                return (new ChangePasswordResult() { Success = true, Message = "Successfully changed password" });
            }
            else
            {
                throw new ApplicationException("User role invalid");
            }
        }
        public async Task<bool> DeactivateAccount(string userId, string userRole)
        {
            if (userRole == "talent")
            {
                var user = (await _userRepository.Get(x => x.Id == userId)).FirstOrDefault();
                user.IsDeleted = true;
                await _userRepository.Update(user);
            }
            else if (userRole == "employer")
            {
                var user = (await _employerRepository.Get(x => x.Id == userId)).FirstOrDefault();
                user.IsDeleted = true;
                await _employerRepository.Update(user);
            }
            else if (userRole == "recruiter")
            {
                var user = (await _recruitorRepository.Get(x => x.Id == userId)).FirstOrDefault();
                user.IsDeleted = true;
                await _recruitorRepository.Update(user);
            }
            else
            {
                throw new ApplicationException("User role invalid");
            }
            return (true);

        }
        public enum UserRole { Talent, Employer, Recruiter }

        public string GetUserRoleString(UserRole userRole)
        {
            string myString = "";
            switch (userRole)
            {
                case UserRole.Talent:
                    myString = "talent";
                    break;
                case UserRole.Employer:
                    myString = "employer";
                    break;
                case UserRole.Recruiter:
                    myString = "recruiter";
                    break;
            }
            return myString;
        }

        public async Task<string> AddEmployer(UpdateClientViewModal user)
        {
            var UId = Guid.NewGuid();
            var objectId = ObjectId.GenerateNewId().ToString();
            // hash password
            var passHash = _encryptPassword.CreateHash(Guid.NewGuid().ToString());//random temperory password

            var login = new Login()
            {
                Id = objectId,
                UId = UId,
                Username = user.Email,
                PasswordHash = passHash,
                IsDisabled = false,
                EmailAddressAuthorized = false,
                ExpiredOn = DateTime.UtcNow.AddHours(24),
                PasswordFormat = PBKDF2_ITERATIONS,
                TermsAccepted = false,//get employers to accept it when they login to verify email
                AccountLocked = true
            };

            var newEmployer = new Employer()
            {
                Id = objectId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                UId = UId,
                Login = login
            };
            newEmployer.CompanyContact.Email = user.Email;
            newEmployer.CompanyContact.Name = user.CompanyName;

            await _employerRepository.Add(newEmployer);

            return newEmployer.Id;
        }

        public async Task<bool> IsRecruiterAuthorised(string clientId, string recruiterId)
        {
            var recruiterClients = (await _recruitorRepository.GetByIdAsync(recruiterId)).Clients;
            var isAuth = recruiterClients.Select(x => x.EmployerId == clientId);

            return isAuth.Count() > 0;
        }

        public async Task<Client> AddEmployerAsClientAsync(string employerId, string recruiterId)
        {
            var employer = await _employerRepository.GetByIdAsync(employerId);
            var recruiter = (await _recruitorRepository.GetByIdAsync(recruiterId));

            Client newClient = new Client
            {
                EmployerId = employer.Id,
                CreatedOn = DateTime.UtcNow,
                InvitationStatus = InvitationStatus.Created
            };

            recruiter.Clients.Add(newClient);

            await _recruitorRepository.Update(recruiter);

            return newClient;
        }

        public async Task RemoveClientFromList(string clientId, string recruiterID)
        {
            var recruiter = (await _recruitorRepository.GetByIdAsync(recruiterID));

            var newClientList = new List<Client>();
            foreach (Client client in recruiter.Clients)
            {
                if (client.EmployerId != clientId)
                    newClientList.Add(client);
            }
            recruiter.Clients = newClientList;
            await _recruitorRepository.Update(recruiter);
        }

        public async Task<bool> ConfirmEmailForClient(ClientInvitationViewModal command)
        {
            var employer = await _employerRepository.GetByIdAsync(command.ClientId);
            
            if (employer.Login.Username != command.ConfirmedEmail)
            {
                if(!await UniqueEmail(command.ConfirmedEmail))
                {
                    return false;
                }
                    employer.Login.Username = command.ConfirmedEmail;
                await _employerRepository.Update(employer);
                
            }
            return true;
        }

        public async Task<Employer> ResetEmployerPassword(string email)
        {
            var user = _employerRepository.GetQueryable().Where(x => x.Login.Username.Equals(email)).FirstOrDefault();
            string token;

            //generate token and update reset token and expiry date
            if (user != null)
            {
                token = await GeneratePasswordResetTokenAsync(user.Id);
                user.Login.ResetPasswordToken = token;
                user.Login.ResetPasswordTokenExpiryDate = DateTime.UtcNow.AddDays(2);
                await _employerRepository.Update(user);
            }
            return user;
        }

        public async Task UpdateClientStatus(string recruiterId, string clientId, InvitationStatus status)
        {
            var recruiter = await _recruitorRepository.GetByIdAsync(recruiterId);

            recruiter.Clients.FirstOrDefault(x => x.EmployerId == clientId)
                .InvitationStatus = status;

            await _recruitorRepository.Update(recruiter);
        }

        public async Task<Recruiter> GetRecruiterFromEmail(string email)
        {
            return (await _recruitorRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
        }

        public async Task<Employer> GetEmployerFromEmail(string email)
        {
            return (await _employerRepository.Get(x => x.Login.Username == email)).FirstOrDefault();
        }


    }
}
