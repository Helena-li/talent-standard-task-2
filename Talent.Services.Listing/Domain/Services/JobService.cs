
using Talent.Services.Talent.Domain.Contracts;
using Talent.Common.Contracts;
using Talent.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Talent.Services.Talent.Domain.Services
{
    public class JobService:IJobService
    {
        IRepository<Employer> _employerRepository;
        IFileService _fileService;
        IRepository<Job> _jobRepository;
        public JobService(IRepository<Job> jobRepository,
                              IRepository<Employer> employerRepository,
                              IFileService fileService)
        { 
            _employerRepository = employerRepository;
            _fileService = fileService;
            _jobRepository = jobRepository;
        }

        public string CreateJob(Job jobData)
        {
            _jobRepository.Add(jobData);
            return jobData.Id;
        } 

        public void UpdateJob(Job jobData)
        {
            _jobRepository.Update(jobData);
        }

        public async Task<Job> GetJobByIDAsync(string id)
        {
            return await _jobRepository.GetByIdAsync(id);
        }
        public async Task<Job> GetJobForTalentMatching(string id, string recruiterId)
        {
            var job =  await _jobRepository.GetByIdAsync(id);
            job.TalentSuggestions = job.TalentSuggestions.Where(x => x.SuggestedBy == recruiterId).ToList();

            return job;
        }
        public async Task<IEnumerable<Job>> GetEmployerJobsAsync(string employerId)
        {
            return await _jobRepository.Get(x => x.EmployerID == employerId);
        }

        public async Task UpdateJobStatusAsync(string jobId, JobStatus status)
        {
            var job =(await GetJobByIDAsync(jobId));
            job.Status = status;
            await _jobRepository.Update(job);
        }
        
    }
}
