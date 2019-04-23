using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Api.Domain.Models;
using Talent.Common.Models;

namespace Talent.Api.Domain.Contracts
{
    public interface IProfileService
    {
        Task<bool> AddTalentVideo(IFormFile file, string talentId);

        Task<TalentProfileViewModel> GetTalentProfile(string Id);
        Task<IEnumerable<TalentVideoMobileViewModel>> GetTalentVideoFeed(string employerId, FeedIncrementModel model);
        Task<TalentSnapshotMobileViewModel> GetTalentSnapshot(string talentId);
    }
}
