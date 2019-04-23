using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Security;
using Talent.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Talent.Common.Contracts;
using MongoDB.Driver;
using Talent.Services.Talent.Domain.Contracts;
using Talent.Services.Profile.Domain.Contracts;

namespace Talent.Services.Listing.Controllers
{
    [Route("listing/[controller]")]
    public class ListingController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IFileService _documentService;
        private readonly IUserAppContext _userAppContext;
        private readonly IRepository<User> _userRepository;
        private readonly IJobService _jobService;
        private readonly IProfileService _profileService;
        private readonly ITalentService _talentService;
        public ListingController(
              IBusClient busClient,
              IAuthenticationService authenticationService,
              IFileService documentService,
              IUserAppContext userAppContext,
              IRepository<User> userRepository,
              IJobService jobService,
              IProfileService profileService,
              ITalentService talentService)
        {
            _busClient = busClient;
            _authenticationService = authenticationService;
            _documentService = documentService;
            _userAppContext = userAppContext;
            _userRepository = userRepository;
            _jobService = jobService;
            _profileService = profileService;
            _talentService = talentService;
        }

        [HttpPost("createUpdateJob")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public IActionResult CreateUpdateJob([FromBody]Job jobData)
        {
            try
            {
                string message = "";
                if (jobData.Id == "")
                {
                    jobData.EmployerID = _userAppContext.CurrentUserId;
                    jobData.Status = JobStatus.Active;
                    jobData.CreatedOn = DateTime.UtcNow;
                    string newJobID = _jobService.CreateJob(jobData);
                    message = "Job added successfully with id " + newJobID;
                }
                else
                {
                    string employerId = _jobService.GetJobByIDAsync(jobData.Id).Result.EmployerID;
                    if (employerId == jobData.EmployerID)
                    {
                        _jobService.UpdateJob(jobData);
                        jobData.Status = JobStatus.Active;
                        message = "Job updated successfully";
                    }
                    else
                        return Json(new { Success = false, Message = "You are not authorised to update this job" });
                }
                return Json(new { Success = true, Message = message });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while adding/updating job" });
            }
        }

        [HttpGet("GetJobByToEdit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public IActionResult GetJobByToEdit(string Id)
        {
            try
            {
                var jobData = _jobService.GetJobByIDAsync(Id).Result;
                if (jobData.EmployerID == _userAppContext.CurrentUserId && jobData.Status == JobStatus.Active)
                    return Json(new { Success = true, jobData });
                else
                    return Json(new { Success = false, Message = "This job is either closed or you are not authorised to edit this job" });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retrieving data" });
            }
        }

        [HttpGet("GetJobForCopy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public IActionResult GetJobForCopy(string Id)
        {
            try
            {
                var jobData = _jobService.GetJobByIDAsync(Id).Result;
                jobData.Id = "";
                if (jobData.EmployerID == _userAppContext.CurrentUserId)
                    return Json(new { Success = true, jobData });
                else
                    return Json(new { Success = false, Message = "This job is either closed or you are not authorised to edit this job" });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retrieving data" });
            }
        }
        [HttpGet("getJobById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public IActionResult GetJobById(string Id)
        {
            try
            {
                var jobData = _jobService.GetJobByIDAsync(Id).Result;
                return Json(new { Success = true, jobData });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retrieving data" });
            }
        }
        [HttpGet("getJobForTalentMatching")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult getJobForTalentMatching(string Id)
        {
            try
            {
                var jobData = _jobService.GetJobForTalentMatching(Id, _userAppContext.CurrentUserId).Result;
                return Json(new { Success = true, jobData });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retrieving data" });
            }
        }

        [HttpGet("getEmployerJobs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetEmployerJobs(string employerId=null)
        {
            try
            {
                employerId =employerId==null? _userAppContext.CurrentUserId:employerId;
                var myJobs = (await _jobService.GetEmployerJobsAsync(employerId)).Select(x => new { x.Id, x.Title, x.Summary, x.JobDetails.Location, x.Status, noOfSuggestions=x.TalentSuggestions!=null && x.TalentSuggestions.Count!=0 ?x.TalentSuggestions.Count:0 });
                return Json(new { Success = true, MyJobs = myJobs });
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retriving Jobs" });
            }
        }

        [HttpGet("getSortedEmployerJobs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetSortedEmployerJobs(int activePage, string sortbyDate, bool showActive, bool showClosed, bool showDraft, bool showExpired, bool showUnexpired, string employerId = null, int limit = 6)
        {
            try
            {
                employerId = employerId == null ? _userAppContext.CurrentUserId : employerId;
                var sortedJobs = (await _jobService.GetEmployerJobsAsync(employerId));

                if (!showActive)
                {
                    sortedJobs = sortedJobs.Where(x => x.Status != JobStatus.Active);
                }

                if(!showClosed)
                {
                    sortedJobs = sortedJobs.Where(x => x.Status != JobStatus.Closed);
                }

                if (!showExpired)
                {
                    sortedJobs = sortedJobs.Where(x => x.ExpiryDate >= DateTime.UtcNow);
                }

                if (!showUnexpired)
                {
                    sortedJobs = sortedJobs.Where(x => x.ExpiryDate < DateTime.UtcNow);
                }

                //TODO Draft not implemented yet
                //if (!showDraft)
                //{

                //}

                if (sortbyDate == "desc")
                {
                    var returnJobs = sortedJobs.OrderByDescending(x => x.CreatedOn).Skip((activePage - 1) * limit).Take(limit)
                        .Select(x => new { x.Id, x.Title, x.Summary, x.JobDetails.Location, x.ExpiryDate, x.Status, noOfSuggestions = x.TalentSuggestions != null && x.TalentSuggestions.Count != 0 ? x.TalentSuggestions.Count : 0 });
                    return Json(new { Success = true, MyJobs = returnJobs, TotalCount = sortedJobs.Count() });
                }

                else
                {
                    var returnJobs = sortedJobs.OrderBy(x => x.CreatedOn).Skip((activePage - 1) * limit).Take(limit)
                        .Select(x => new { x.Id, x.Title, x.Summary, x.JobDetails.Location, x.ExpiryDate, x.Status, noOfSuggestions = x.TalentSuggestions != null && x.TalentSuggestions.Count != 0 ? x.TalentSuggestions.Count : 0 });
                    return Json(new { Success = true, MyJobs = returnJobs, TotalCount = sortedJobs.Count() });
                }                
            }
            catch
            {
                return Json(new { Success = false, Message = "Error while retriving Jobs" });
            }
        }
        [HttpPost("closeJob")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> CloseJob([FromBody]string id)
        {
            try
            {
                //userId is either Employer or Recruiter
                string userId = (await _jobService.GetJobByIDAsync(id)).EmployerID;
                
                if (userId == _userAppContext.CurrentUserId)
                {
                    var jobStatus = JobStatus.Closed;
                    await _jobService.UpdateJobStatusAsync(id, jobStatus);
                    return Json(new { Success = true, Message = "Job updated successfully" });
                }
                else
                    return Json(new { Success = false, Message = "You are not authorised to update this job" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = "Error while updating job" });
            }
        }

        [HttpGet("getWatchlistIds")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetTalentWatchlistIds()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getWatchlist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetTalentWatchlist()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("watchTalent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> SetWatchlistStatusForTalent(string talentId, bool isWatching)
        {
            //Your code here;
            throw new NotImplementedException();
        }
    }
}
