using Talent.Common.Commands;
using Talent.Common.Models;
using Talent.Common.Security;
using Talent.Services.Identity.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Talent.Common.Auth;
using Talent.Services.Identity.Domain.Models;
using Talent.Services.Identity.Domain.Models.Client;

namespace Talent.Services.Identity.Controllers
{
    [Route("authentication/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserAppContext _userAppContext;
        public AuthenticationController(
              IBusClient busClient,
              IAuthenticationService authenticationService,
              IUserAppContext userAppContext)
        {
            _busClient = busClient;
            _authenticationService = authenticationService;
            _userAppContext = userAppContext;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]CreateUser command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { IsSuccess = false, Message = "Parameter can not be null" });
                }

                command.Email = command.Email.ToLower();

                //check if username is unique
                bool isUniqueEmail = await _authenticationService.UniqueEmail(command.Email);
                if (!isUniqueEmail)
                {
                    return Json(new { IsSuccess = false, Message = "This email is currently in use" });
                }

                var authenticatedToken = new JsonWebToken();
                authenticatedToken = await _authenticationService.SignUp(command);

                return Json(new { IsSuccess = true, Message = "Sign up complete." });
            }
            catch (ApplicationException e)
            {
                return Json(new { IsSuccess = false, e.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]CreateUser command)
        {
            try
            {
                command.Email = command.Email.ToLower();
                var authenticateUser = await _authenticationService.LoginAsync(command.Email, command.Password);

                if (!await _authenticationService.IsEmailVerified(command.Email, authenticateUser.UserRole))
                    return Json(new { IsSuccess = true, isEmailVerified = false, Message = "Please Verifiy Your Email" });

                return Json(new { IsSuccess = true, isEmailVerified = true, Token = authenticateUser });
            }
            catch (ApplicationException e)
            {
                return Json(new { IsSuccess = false, e.Message });
            }
        }

      

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            var userId = _userAppContext.CurrentUserId;
            return Content("Secured");
        }

        

       
        [HttpPost("verifyResetPasswordToken")]
        public async Task<ActionResult> VerifyResetPasswordToken([FromQuery]string o, [FromQuery]string p)
        {
            try
            {
                bool isTokenValid = await _authenticationService.VerifyToken(o, p);
                return Json(new { Success = true, isTokenValid });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }


       

        [HttpGet("getAccountSettingInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAccountSettingInfo()
        {
            var userRole = _userAppContext.CurrentRole;
            var userId = _userAppContext.CurrentUserId;
            var userName = _authenticationService.GetAccountSetting(userId, userRole);
            return Json(new { userRole, userId, userName });
        }

        [HttpPost("changeUserName")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangeUserName(string userName)
        {
            var userRole = _userAppContext.CurrentRole;
            var userId = _userAppContext.CurrentUserId;
            var newName = await _authenticationService.ChangeUserName(userId, userRole, userName);
            return Json(new { newName });
        }
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
        {
            var userRole = _userAppContext.CurrentRole;
            var userId = _userAppContext.CurrentUserId;
            var newPassword = await _authenticationService.ChangePassword(userId, userRole, model);
            return Json(new { Success = newPassword });
        }

        [HttpPost("deactivateAccount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeactivateAccount()
        {
            var userRole = _userAppContext.CurrentRole;
            var userId = _userAppContext.CurrentUserId;
            try
            {
                var deactivate = await _authenticationService.DeactivateAccount(userId, userRole);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        #region ManageClient
        [HttpPost("addNewClient")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> AddNewClient([FromBody]UpdateClientViewModal command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Success = false, Message = "Parameter can not be null" });
                }

                string EmployerId = "";
                if (command.ClientId == null)
                {
                    //add client
                    command.Email = command.Email.ToLower();

                    //check if username is unique
                    bool isUniqueEmail = await _authenticationService.UniqueEmail(command.Email);
                    if (!isUniqueEmail)
                    {
                        return Json(new { Success = false, Message = "This email is currently in use" });
                    }

                    EmployerId = await _authenticationService.AddEmployer(command);

                    var newClient = (await _authenticationService.AddEmployerAsClientAsync(EmployerId, _userAppContext.CurrentUserId));

                    return Json(new { Success = true, Message = "Successful", newClient });
                }
                return Json(new { Success = false, Message = "OOPS! Something Went Wrong" });
            }
            catch (ApplicationException e)
            {
                return Json(new { IsSuccess = false, e.Message });
            }
        }

        [HttpPost("deactivateClientAccount")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> DeactivateClientAccount(string id)
        {
            var userRole = "employer";
            var userId = id;
            try
            {
                if (await _authenticationService.IsRecruiterAuthorised(id, _userAppContext.CurrentUserId))
                {
                    var deactivate = await _authenticationService.DeactivateAccount(userId, userRole);
                    if (deactivate == true)
                        await _authenticationService.RemoveClientFromList(id, _userAppContext.CurrentUserId);

                    return Json(new { Success = true, Message = "Account Deactivated Successfully" });
                }
                else
                {
                    return Json(new { Success = false, Message = "You are not authorised to Delete this account" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = e.Message });
            }
        }




        

        [HttpPost("verifyClientToken")]
        public async Task<ActionResult> VeifyClientToken([FromQuery]string recruiterEmail, [FromQuery]string clientEmail, [FromQuery]string resetPasswordToken)
        {
            try
            {
                bool isTokenValid = false;
                var recruiter = new Recruiter();
                var client = new Employer();
                bool isClientAuthorised = false;
                if (recruiterEmail != null)
                {
                    recruiter = await _authenticationService.GetRecruiterFromEmail(recruiterEmail);
                }
                if (clientEmail != null)
                {
                    client = await _authenticationService.GetEmployerFromEmail(clientEmail);
                }

                if (recruiter != null && client != null)
                {
                    isClientAuthorised = await _authenticationService.IsRecruiterAuthorised(client.Id, recruiter.Id);
                    if (!isClientAuthorised)
                    {
                        throw new ArgumentException("Token is invalid or has expired please contact your recruiter");
                    }
                    isTokenValid = await _authenticationService.VerifyToken(clientEmail, resetPasswordToken);
                }
                return Json(new { Success = true, isTokenValid });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        
        #endregion
    }
}
