using Talent.Common.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Models
{
    public class UserSkillListing: IMongoCommon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SellerId { get; set; }
        public decimal Charge { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsTradable { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
