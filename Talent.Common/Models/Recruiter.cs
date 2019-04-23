using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Talent.Common.Models
{
    public class MatchingPool
    {
        string[] EmployerID { get; set; }
        string[] JobID { get; set; }
    }

    public enum InvitationStatus { Created, Sent, Active };

    public class Client
    {
        public String EmployerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public InvitationStatus InvitationStatus { get; set; }
        //talent count get it from employer table 
    }
    
    public class Recruiter : Employer
    {
        public MatchingPool Matches { get; set; }
        public ICollection<Client> Clients { get; set; }
        public Recruiter()
        {
            Clients = new List<Client>();
        }
    }
}
