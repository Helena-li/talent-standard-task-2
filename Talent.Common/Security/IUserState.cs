using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Security
{
    public interface IUserState
    {
        ObjectId Id { get; set; }
        Guid UId { get; set; }

        string Language { get; set; }

        string Culture { get; set; }

        string TimezoneIdentifier { get; }
        string Role { get; }
    }

    public class UserState : IUserState
    {
        //public UserState(int id, Org org, string language, string culture, string timezoneIdentifier)
        //{
        //    Id = id;
        //    Org = org;
        //    Language = language;
        //    Culture = culture;
        //    TimezoneIdentifier = timezoneIdentifier;
        //}
        public ObjectId Id
        {
            get;
            set;
        }

        public Guid UId
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public string Culture
        {
            get;
            set;
        }

        public string TimezoneIdentifier
        {
            get;
            set;
        }

        public string Role
        {
            get;
            set;
        }
    }
}
