using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Models
{
    [BsonIgnoreExtraElements]
    public class TalentSuggestion
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string TalentId { get; set; }
        public string SuggestedBy { get; set; }
        public string Notes { get; set; }
        public string Comments { get; set; }
    }
}
