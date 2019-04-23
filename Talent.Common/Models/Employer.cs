using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Models
{
    public class Employer : IMongoCommon
    {
        public Guid UId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public ContactDetail CompanyContact { get; set; }
        public ContactDetail PrimaryContact { get; set; }    
        public string ProfilePhoto { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public List<UserSkill> Skills { get; set; }
        public bool DisplayProfile { get; set; }

        public string VideoName { get; set; }
        public ICollection<TalentVideo> Videos { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<TalentSuggestion> Suggestions { get; set; }
        public ICollection<string> TalentWatchlist { get; set; }

        public Login Login { get; set; }

        public Employer()
        {
            CompanyContact = new ContactDetail();
            PrimaryContact = new ContactDetail();
            Skills = new List<UserSkill>();
            Videos = new List<TalentVideo>();
            Suggestions = new List<TalentSuggestion>();
            TalentWatchlist = new List<string>();
        }
    }

    public class Tag
    {
        public String Id { get; set; }
        public String Text { get; set; }
    }

    public class ContactDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Location Location { get; set; }
        public ContactDetail()
        {
            Location = new Location();
        }
    }
}

