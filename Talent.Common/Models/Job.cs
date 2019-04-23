using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Talent.Common.Models
{
    public enum JobStatus
    {
        Active, Closed
    }
    
    public class Job:IMongoCommon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string EmployerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ApplicantDetails ApplicantDetails { get; set; }
        public JobDetails JobDetails { get; set; }
        public JobStatus Status { get; set; }
        public ICollection<TalentSuggestion> TalentSuggestions { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ApplicantDetails
    {
        public ExperienceRequired YearsOfExperience { get; set; }
        public string[] Qualifications { get; set; }
        public string[] VisaStatus { get; set; }
    }

    public class ExperienceRequired
    {
        public int Years { get; set; }
        public int Months { get; set; }
    }

    public class JobDetails
    {
        public JobCategory Categories { get; set; }
        public string[] JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Salary Salary { get; set; }
        public Location Location { get; set; }
    }

    public class JobCategory
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }

    public class Salary
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
    }
}


/*jobData: {
                title: "",
                description: "",
                logoUrl: "",
                summary: "",
                applicantDetails: {
                    yearsOfExperience: { years: 0, months: 0 },
                    qualifications: [],
                    visaStatus:[]
                },
                jobDetails: {
                    categories: { category: "", subCategory: "" },
                    jobType: [],
                    startDate: moment(),
                    salary: { from: 0, to: 0 },
                    location: { country: "", city: ""}
                }
            }
*/
