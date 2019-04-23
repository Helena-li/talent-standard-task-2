using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Talent.Api.Domain.Contracts;
using Talent.Common.Aws;
using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Common.Security;

namespace Talent.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/mobile")]
    public class MobileController : Controller
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
        private readonly IAwsService _awsService;
        private readonly string _profileImageFolder;

        public MobileController(IBusClient busClient,
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
            IHostingEnvironment environment,
            IProfileService profileService,
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
            _environment = environment;
            _profileImageFolder = "images\\";
            _awsService = awsService;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Content("Hello from Profile Api");
        }

        [HttpPost("updateTalentVideo")]
        [DisableRequestSizeLimit]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateTalentVideo()
        {
            try
            {
                string userId = _userAppContext.CurrentUserId;
                var files = Request.Form.Files;
                IFormFile file = files[0];
                if (await _profileService.AddTalentVideo(file, userId))
                {
                    string videoName = (await _profileService.GetTalentProfile(userId)).VideoUrl;
                    return Json(new { Success = true, videoName });
                }
                return Json(new { Success = false, Message = "File save failed" });
            }
            catch (Exception e)
            {
                throw;
            }

        }

        [HttpGet("getTalentVideoList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTalentVideoList(FeedIncrementModel increment)
        {
            try
            {
                string employerId = _userAppContext.CurrentUserId;
                return Json(new { Success = true, Data = await _profileService.GetTalentVideoFeed(employerId, increment) });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpGet("getTalentSnapshot")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTalentSnapshot(string talentId)
        {
            try
            {
                return Json(new { Success = true, Data = await _profileService.GetTalentSnapshot(talentId) });
            }
            catch (Exception e)
            {
                return Json( new { Success = false, e.Message } );
            }
        }
    }
}