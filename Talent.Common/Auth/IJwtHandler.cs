using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Talent.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(string userId, string userRole, bool isSignUp);
    }
}
