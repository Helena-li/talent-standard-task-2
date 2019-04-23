using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talent.Common.Models
{
    public class Login : IMongoCommon
    {
        public Guid UId { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
        public int? FailedLoginCount { get; set; }
        public bool? AccountLocked { get; set; }
        public DateTime? LastSuccessfulLogin { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int PasswordFormat { get; set; }
        public int? PasswordHashIterations { get; set; }
        public bool EmailAddressAuthorized { get; set; }
        public bool TermsAccepted { get; set; }
        public string AuthUserUrn { get; set; }
        public Guid? EmailCode { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ExpiredOn { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiryDate { get; set; }

    }
}
