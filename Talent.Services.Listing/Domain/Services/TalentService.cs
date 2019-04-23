using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Common.Security;
using Talent.Services.Talent.Domain.Contracts;

namespace Talent.Services.Talent.Domain.Services
{
    public class TalentService : ITalentService
    {
        private readonly IRepository<Employer> _employerRepository;
        private readonly IRepository<User> _talentRepository;
        private readonly IRepository<Recruiter> _recruiterRepository;
        private readonly IUserAppContext _userAppContext;

        public TalentService(IRepository<Employer> employerRepository,
            IRepository<User> talentRepository,
            IRepository<Recruiter> recruiterRepository,
            IUserAppContext userAppContext
            )
        {
            _employerRepository = employerRepository;
            _recruiterRepository = recruiterRepository;
            _userAppContext = userAppContext;
            _talentRepository = talentRepository;
        }

        public async Task<IEnumerable<string>> GetTalentWatchlistIds(string employerId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task SetWatchingTalent(string employerId, string talentId, bool isWatching)
        {
            //Your code here;
            throw new NotImplementedException();
        }
    }
}
