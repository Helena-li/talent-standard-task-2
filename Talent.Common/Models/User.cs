using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Common.Models
{
    public class User : IMongoCommon
    {
        public Guid UId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public bool IsMobilePhoneVerified { get; set; }

        public Address Address { get; set; }
        public string Nationality { get; set; }
        public string VisaStatus { get; set; }
        public JobSeekingStatus JobSeekingStatus { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public List<UserLanguage> Languages { get; set; }
        public List<UserSkill> Skills { get; set; }
        public List<UserEducation> Education { get; set; }
        public List<UserCertification> Certifications { get; set; }
        public List<UserExperience> Experience { get; set; }
        public UserLocation Location { get; set; }
        public string ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string VideoName { get; set; }
        public ICollection<TalentVideo> Videos { get; set; }
        public string CvName { get; set; }
        public LinkedAccounts LinkedAccounts { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Login Login { get; set; }

        public User()
        {
            Languages = new List<UserLanguage>();
            Skills = new List<UserSkill>();
            Education = new List<UserEducation>();
            Certifications = new List<UserCertification>();
            Experience = new List<UserExperience>();
            Address = new Address();
            Videos = new List<TalentVideo>();
            LinkedAccounts = new LinkedAccounts();
        }
    }

    public class Address
    {
        public string Number { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public Address()
        {
            Number = "";
            Street = "";
            Suburb = "";
            PostCode = 0;
            City = "";
            Country = "";
        }
    }

    public class LinkedAccounts
    {
        public string LinkedIn { get; set; }
        public string Github { get; set; }

        public LinkedAccounts()
        {
            LinkedIn = "";
            Github = "";
        }
    }

    public class JobSeekingStatus
    {
        public string Status { get; set; }
        public DateTime? AvailableDate { get; set; }

        public JobSeekingStatus()
        {
            Status = "";
            AvailableDate = null;
        }
    }
}
