using System.Collections.Generic;
using System.Threading.Tasks;
using Talent.Common.Models;

namespace Talent.Services.Talent.Domain.Contracts
{
    public interface IJobService
    {
        string CreateJob(Job jobData);
        void UpdateJob(Job jobData);
        Task<Job> GetJobByIDAsync(string id);
        Task<Job> GetJobForTalentMatching(string id, string recruiterId);
        Task<IEnumerable<Job>> GetEmployerJobsAsync(string employerId);
        Task UpdateJobStatusAsync(string jobId, JobStatus status);
    }
}
