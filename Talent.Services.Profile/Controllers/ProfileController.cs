using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Common.Security;
using Talent.Services.Profile.Models.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using Talent.Services.Profile.Domain.Contracts;
using Talent.Common.Aws;
using Talent.Services.Profile.Models;

namespace Talent.Services.Profile.Controllers
{
    [Route("profile/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProfileService _profileService;
        private readonly IFileService _documentService;
        private readonly IUserAppContext _userAppContext;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserLanguage> _userLanguageRepository;
        private readonly IRepository<UserDescription> _personDescriptionRespository;
        private readonly IRepository<UserAvailability> _userAvailabilityRepository;
        private readonly IRepository<UserSkill> _userSkillRepository;
        private readonly IRepository<UserEducation> _userEducationRepository;
        private readonly IRepository<UserCertification> _userCertificationRepository;
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IRepository<Employer> _employerRepository;
        private readonly IRepository<UserDocument> _userDocumentRepository;
        private readonly IHostingEnvironment _environment;
        private readonly IRepository<Recruiter> _recruiterRepository;
        private readonly IAwsService _awsService;
        private readonly string _profileImageFolder;

        public ProfileController(IBusClient busClient,
            IProfileService profileService,
            IFileService documentService,
            IRepository<User> userRepository,
            IRepository<UserLanguage> userLanguageRepository,
            IRepository<UserDescription> personDescriptionRepository,
            IRepository<UserAvailability> userAvailabilityRepository,
            IRepository<UserSkill> userSkillRepository,
            IRepository<UserEducation> userEducationRepository,
            IRepository<UserCertification> userCertificationRepository,
            IRepository<UserLocation> userLocationRepository,
            IRepository<Employer> employerRepository,
            IRepository<UserDocument> userDocumentRepository,
            IRepository<Recruiter> recruiterRepository,
            IHostingEnvironment environment,
            IAwsService awsService,
            IUserAppContext userAppContext)
        {
            _busClient = busClient;
            _profileService = profileService;
            _documentService = documentService;
            _userAppContext = userAppContext;
            _userRepository = userRepository;
            _personDescriptionRespository = personDescriptionRepository;
            _userLanguageRepository = userLanguageRepository;
            _userAvailabilityRepository = userAvailabilityRepository;
            _userSkillRepository = userSkillRepository;
            _userEducationRepository = userEducationRepository;
            _userCertificationRepository = userCertificationRepository;
            _userLocationRepository = userLocationRepository;
            _employerRepository = employerRepository;
            _userDocumentRepository = userDocumentRepository;
            _recruiterRepository = recruiterRepository;
            _environment = environment;
            _profileImageFolder = "images\\";
            _awsService = awsService;
        }

        #region Talent

        [HttpGet("getProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userAppContext.CurrentUserId;
            var user = await _userRepository.GetByIdAsync(userId);
            return Json(new { Username = user.FirstName });
        }

        [HttpGet("getProfileById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetProfileById(string uid)
        {
            var userId = uid;
            var user = await _userRepository.GetByIdAsync(userId);
            return Json(new { userName = user.FirstName, createdOn = user.CreatedOn });
        }

        [HttpGet("isUserAuthenticated")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> IsUserAuthenticated()
        {
            if (_userAppContext.CurrentUserId == null)
            {
                return Json(new { IsAuthenticated = false });
            }
            else
            {
                var person = await _userRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (person != null)
                {
                    return Json(new { IsAuthenticated = true, Username = person.FirstName, Type = "talent" });
                }
                var employer = await _employerRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (employer != null)
                {
                    return Json(new { IsAuthenticated = true, Username = employer.CompanyContact.Name, Type = "employer" });
                }
                var recruiter = await _recruiterRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (recruiter != null)
                {
                    return Json(new { IsAuthenticated = true, Username = recruiter.CompanyContact.Name, Type = "recruiter" });
                }
                return Json(new { IsAuthenticated = false, Type = "" });
            }
        }

        [HttpGet("getLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetLanguages()
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if (userProfile.Languages==null)
                {
                    return Json(new { Success = false, data = "language is null." });
                }
                return Json(new { Success = true, data = userProfile.Languages });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("addLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> AddLanguage([FromBody] AddLanguageViewModel language)
        {
            try
            {
                var result = await _profileService.AddNewLanguage(language);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not add the language." });
                }
                return Json(new { Success = true, data = "add successful" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("updateLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateLanguage([FromBody] AddLanguageViewModel language)
        {
            try
            {
                var result = await _profileService.UpdateLanguage(language);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not update the language." });
                }
                return Json(new { Success = true, data = "update the language successful." });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("deleteLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> DeleteLanguage([FromBody] AddLanguageViewModel language)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    var findLanguageItem = user.Languages.SingleOrDefault(x => x.Id == language.Id);
                    if (findLanguageItem!=null)
                    {
                        findLanguageItem.IsDeleted = true;
                        user.Languages[user.Languages.FindIndex(x => x.Id == language.Id)] = findLanguageItem;
                        await _userRepository.Update(user);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Can't find the language!" });
                    }
                    
                }
                else
                {
                    return Json(new { Success = false, Message="Can't find the user!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Message = "error" });
            }
            //throw new NotImplementedException();
        }

        [HttpGet("getSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if (userProfile.Skills == null)
                {
                    return Json(new { Success = false, data = "Skill is null." });
                }
                return Json(new { Success = true, data = userProfile.Skills });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("addSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddSkill([FromBody]AddSkillViewModel skill)
        {
            try
            {
                var result = await _profileService.AddNewSkill(skill);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not add the skill." });
                }
                return Json(new { Success = true, data = "add successful" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("updateSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateSkill([FromBody]AddSkillViewModel skill)
        {
            try
            {
                var result = await _profileService.UpdateSkill(skill);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not update the skill." });
                }
                return Json(new { Success = true, data = "update the skill successful." });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("deleteSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteSkill([FromBody]AddSkillViewModel skill)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    var findSkillItem = user.Skills.SingleOrDefault(x => x.Id == skill.Id);
                    if (findSkillItem != null)
                    {
                        findSkillItem.IsDeleted = true;
                        user.Skills[user.Skills.FindIndex(x => x.Id == skill.Id)] = findSkillItem;
                        await _userRepository.Update(user);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Can't find the language!" });
                    }

                }
                else
                {
                    return Json(new { Success = false, Message = "Can't find the user!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Message = "error" });
            }
        }

        [HttpGet("getExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetExperience()
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if (userProfile.Experience == null)
                {
                    return Json(new { Success = false, data = "Skill is null." });
                }
                return Json(new { Success = true, data = userProfile.Experience });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("addExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddExperience([FromBody]ExperienceViewModel experience)
        {
            try
            {
                var result = await _profileService.AddNewExperience(experience);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not add the experience." });
                }
                return Json(new { Success = true, data = "add successful" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("updateExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateExperience([FromBody]ExperienceViewModel experience)
        {
            try
            {
                var result = await _profileService.UpdateExperience(experience);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not update the experience." });
                }
                return Json(new { Success = true, data = "update the experience successful." });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("deleteExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteExperience([FromBody]ExperienceViewModel experience)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    var findExperienceItem = user.Experience.SingleOrDefault(x => x.Id == experience.Id);
                    if (findExperienceItem != null)
                    {
                        user.Experience.Remove(findExperienceItem);
                        await _userRepository.Update(user);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Can't find the language!" });
                    }

                }
                else
                {
                    return Json(new { Success = false, Message = "Can't find the user!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Message = "error" });
            }
        }

        [HttpGet("getCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> getCertification()
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if (userProfile.Certifications == null)
                {
                    return Json(new { Success = false, data = "Certifications is null." });
                }
                return Json(new { Success = true, data = userProfile.Certifications });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("addCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> addCertification([FromBody] AddCertificationViewModel certificate)
        {
            try
            {
                var result = await _profileService.AddNewCertification(certificate);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not add the certificate." });
                }
                return Json(new { Success = true, data = "add successful" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("updateCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateCertification([FromBody] AddCertificationViewModel certificate)
        {
            try
            {
                var result = await _profileService.UpdateCertification(certificate);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not update the certificate." });
                }
                return Json(new { Success = true, data = "update the certificate successful." });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("deleteCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteCertification([FromBody] AddCertificationViewModel certificate)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    var findCertificationItem = user.Certifications.SingleOrDefault(x => x.Id == certificate.Id);
                    if (findCertificationItem != null)
                    {
                        findCertificationItem.IsDeleted = true;
                        user.Certifications[user.Certifications.FindIndex(x => x.Id == certificate.Id)] = findCertificationItem;
                        await _userRepository.Update(user);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Can't find the language!" });
                    }

                }
                else
                {
                    return Json(new { Success = false, Message = "Can't find the user!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Message = "error" });
            }
        }

        [HttpGet("getProfileImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getProfileImage(string Id)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if ((userProfile.ProfilePhoto == null) || (userProfile.ProfilePhotoUrl == null))
                {
                    return Json(new { Success = false, data = "Profile photo is null." });
                }
                else if (userProfile.ProfilePhoto!=Id)
                {
                    return Json(new { Success = false, data = "can't find the Profile photo with the id." });
                }
                var profileUrl = _documentService.GetFileURL(Id, FileType.ProfilePhoto);
                return Json(new { Success = true, profilePath = profileUrl });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
            
        }

        [HttpPost("updateProfilePhoto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateProfilePhoto()
        {
            try
            {
                var file = Request.Form.Files[0];
                var userId = _userAppContext.CurrentUserId;
                var url = await _profileService.UpdateTalentPhoto(userId, file);
                return url == false ? Json(new { Success = false }) : Json(new { Success = true});
            }
            catch (Exception e)
            {
                return Json(new { Data = "error" });
            }
        }

        [HttpPost("updateTalentCV")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateTalentCV()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getProfileVideo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getProfileVideo(string Id)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if ((userProfile.VideoName == null) || (userProfile.VideoUrl == null))
                {
                    return Json(new { Success = false, data = "Profile photo is null." });
                }
                else if (userProfile.VideoName != Id)
                {
                    return Json(new { Success = false, data = "can't find the Profile photo with the id." });
                }
                var profileUrl = _documentService.GetFileURL(Id, FileType.UserVideo);
                return Json(new { Success = true, profilePath = profileUrl });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }

        }

        [HttpPost("updateTalentVideo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateTalentVideo()
        {
            try
            {
                var file = Request.Form.Files[0];
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else
                {
                    if (userProfile.VideoName != null)
                    {
                        bool removeOld = await _profileService.RemoveTalentVideo(userId, userProfile.VideoName);
                        //if (!removeOld)
                        //{
                        //    return Json(new { Success = false, data = "can not delete the old video." });
                        //}
                    }
                    bool addNew = await _profileService.AddTalentVideo(userId, file);
                    if (!addNew)
                    {
                        return Json(new { Success = false, data = "can not add the new video." });
                    }
                    return Json(new { Success = true });
                }
            }
            catch (Exception e)
            {
                return Json(new { Data = "error" });
            }
        }

        [HttpGet("getInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetInfo()
        {
            //Your code here;
            throw new NotImplementedException();
        }


        [HttpPost("addInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddInfo([FromBody] DescriptionViewModel pValue)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetEducation()
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var userProfile = await _profileService.GetTalentProfile(userId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                else if (userProfile.Education == null)
                {
                    return Json(new { Success = false, data = "Education is null." });
                }
                return Json(new { Success = true, data = userProfile.Education });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("addEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddEducation([FromBody]AddEducationViewModel model)
        {
            try
            {
                var result = await _profileService.AddNewEducation(model);
                if (result==false)
                {
                    return Json(new { Success = false, data = "can not add the education." });
                }
                return Json(new { Success = true, data = "add successful" });
            }
            catch (Exception e)
            {

                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("updateEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateEducation([FromBody]AddEducationViewModel model)
        {
            try
            {
                var result = await _profileService.UpdateEducation(model);
                if (result == false)
                {
                    return Json(new { Success = false, data = "can not update the education." });
                }
                return Json(new { Success = true, data = "update the education successful." });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, data = "error" });
            }
        }

        [HttpPost("deleteEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteEducation([FromBody] AddEducationViewModel model)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    var findEducationItem = user.Education.SingleOrDefault(x => x.Id == model.Id);
                    if (findEducationItem != null)
                    {
                        findEducationItem.IsDeleted = true;
                        user.Education[user.Education.FindIndex(x => x.Id == model.Id)] = findEducationItem;
                        await _userRepository.Update(user);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Can't find the education!" });
                    }

                }
                else
                {
                    return Json(new { Success = false, Message = "Can't find the user!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { Message = "error" });
            }
        }


        #endregion

        #region EmployerOrRecruiter

        [HttpGet("getEmployerProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetEmployerProfile(String id = "", String role = "")
        {
            try
            {
                string userId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;
                string userRole = String.IsNullOrWhiteSpace(role) ? _userAppContext.CurrentRole : role;

                var employerResult = await _profileService.GetEmployerProfile(userId, userRole);

                return Json(new { Success = true, employer = employerResult });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e });
            }
        }

        [HttpPost("saveEmployerProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> SaveEmployerProfile([FromBody] EmployerProfileViewModel employer)
        {
            if (ModelState.IsValid)
            {
                if (await _profileService.UpdateEmployerProfile(employer, _userAppContext.CurrentUserId, _userAppContext.CurrentRole))
                {
                    return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }

        [HttpPost("saveClientProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> SaveClientProfile([FromBody] EmployerProfileViewModel employer)
        {
            if (ModelState.IsValid)
            {
                //check if employer is client 5be40d789b9e1231cc0dc51b
                var recruiterClients = (await _recruiterRepository.GetByIdAsync(_userAppContext.CurrentUserId)).Clients;

                if (recruiterClients.Select(x => x.EmployerId == employer.Id).FirstOrDefault())
                {
                    if (await _profileService.UpdateEmployerProfile(employer, _userAppContext.CurrentUserId, "employer"))
                    {
                        return Json(new { Success = true });
                    }
                }
            }
            return Json(new { Success = false });
        }

        [HttpPost("updateEmployerPhoto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<ActionResult> UpdateEmployerPhoto()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("updateEmployerVideo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> UpdateEmployerVideo()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEmployerProfileImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<ActionResult> GetWorkSample(string Id)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEmployerProfileImages")]
        public ActionResult GetWorkSampleImage(string Id)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #endregion

        #region TalentFeed

        [HttpGet("getTalentProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent, employer, recruiter")]
        public async Task<IActionResult> GetTalentProfile(String id = "")
        {
            try
            {
                String talentId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;

                var userProfile = await _profileService.GetTalentProfile(talentId);
                if (userProfile == null)
                {
                    return Json(new { Success = false, data = "can not find the profile of the id." });
                }
                return Json(new { Success = true, data = userProfile });
            }
            catch (Exception e)
            {

                return Json(new { Success = false, e.Message });
            }

        }

        [HttpPost("updateTalentProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateTalentProfile([FromBody]TalentProfileViewModel profile)
        {

            if (ModelState.IsValid)
            {
                if (await _profileService.UpdateTalentProfile(profile, _userAppContext.CurrentUserId))
                {
                    return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }

        [HttpGet("getTalent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter, employer")]
        public async Task<IActionResult> GetTalentSnapshots(FeedIncrementModel feed)
        {
            try
            {
                var result = (await _profileService.GetTalentSnapshotList(_userAppContext.CurrentUserId, false, feed.Position, feed.Number)).ToList();

                // Dummy talent to fill out the list once we run out of data
                //if (result.Count == 0)
                //{
                //    result.Add(
                //            new Models.TalentSnapshotViewModel
                //            {
                //                CurrentEmployment = "Software Developer at XYZ",
                //                Level = "Junior",
                //                Name = "Dummy User...",
                //                PhotoId = "",
                //                Skills = new List<string> { "C#", ".Net Core", "Javascript", "ReactJS", "PreactJS" },
                //                Summary = "Veronika Ossi is a set designer living in New York who enjoys kittens, music, and partying.",
                //                Visa = "Citizen"
                //            }
                //        );
                //}
                return Json(new { Success = true, Data = result });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
        }
        #endregion

        #region TalentMatching

        [HttpGet("getTalentList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> GetTalentListAsync()
        {
            try
            {
                var result = await _profileService.GetFullTalentList();
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpGet("getEmployerList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetEmployerList()
        {
            try
            {
                var result = _profileService.GetEmployerList();
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("getEmployerListFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetEmployerListFilter([FromBody]SearchCompanyModel model)
        {
            try
            {
                var result = _profileService.GetEmployerListByFilterAsync(model);//change to filters
                if (result.IsCompletedSuccessfully)
                    return Json(new { Success = true, Data = result.Result });
                else
                    return Json(new { Success = false, Message = "No Results found" });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("getTalentListFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetTalentListFilter([FromBody] SearchTalentModel model)
        {
            try
            {
                var result = _profileService.GetTalentListByFilterAsync(model);//change to filters
                return Json(new { Success = true, Data = result.Result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpGet("getSuggestionList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetSuggestionList(string employerOrJobId, bool forJob)
        {
            try
            {
                var result = _profileService.GetSuggestionList(employerOrJobId, forJob, _userAppContext.CurrentUserId);
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("addTalentSuggestions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> AddTalentSuggestions([FromBody] AddTalentSuggestionList talentSuggestions)
        {
            try
            {
                if (await _profileService.AddTalentSuggestions(talentSuggestions))
                {
                    return Json(new { Success = true });
                }

            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
            return Json(new { Success = false });
        }

        #endregion


        #region ManageClients

        [HttpGet("getClientList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> GetClientList()
        {
            try
            {
                var result = await _profileService.GetClientListAsync(_userAppContext.CurrentUserId);

                return Json(new { Success = true, result });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        //[HttpGet("getClientDetailsToSendMail")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        //public async Task<IActionResult> GetClientDetailsToSendMail(string clientId)
        //{
        //    try
        //    {
        //            var client = await _profileService.GetEmployer(clientId);

        //            string emailId = client.Login.Username;
        //            string companyName = client.CompanyContact.Name;

        //            return Json(new { Success = true, emailId, companyName });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Success = false, Message = e.Message });
        //    }
        //}

        #endregion

        public IActionResult Get() => Content("Test");

    }
}
