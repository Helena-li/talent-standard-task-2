using Talent.Common.Contracts;
using Talent.Common.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Talent.Common.Security
{
    public interface IUserAppContext
    {
        IEnumerable<Claim> Claims { get; }
        bool IsAuthenticated { get; }
        string CurrentUserId { get; }
        string CurrentRole { get; }
    }

    public class UserAppContext : IUserAppContext
    {
        #region Data members
        private readonly Func<IPrincipal> _principalFactory;
        private readonly IRepository<User> _userRepository;
        private string _userId;
        private string _userRole;

        #endregion
        public UserAppContext(Func<IPrincipal> principalFactory,
            IRepository<User> userRepository)
        {
            _principalFactory = principalFactory;
            _userRepository = userRepository;
        }

        public bool IsAuthenticated
        {
            get { return _principalFactory().Identity.IsAuthenticated; }
        }

        public IEnumerable<Claim> Claims
        {
            get
            {
                var principal = _principalFactory();
                if (principal == null)
                {
                    throw new ApplicationException("Expected to get a Principle, as we are using IdSrv as our Authentication Provider.");
                }
                var claimsPrincipal = principal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    throw new ApplicationException("Expected to get a ClaimsPrinciple, as we are using IdSrv as our Authentication Provider.");
                }
                return claimsPrincipal.Claims;
            }
        }

        public string CurrentUserId
        {
            get
            {
                var userIdClaim = Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId);
                if (userIdClaim == null)
                {
                    throw new Exception("The _httpContext.Session is null.");
                }

                if (Convert.ToString(userIdClaim.Value) != null)
                {
                    _userId = userIdClaim.Value;
                    return _userId;
                }
                throw new Exception("User Id can not find");
            }
        }

        public string CurrentRole
        {
            get
            {
                var userRoleClaim = Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (userRoleClaim == null)
                {
                    throw new Exception("The _httpContext.Session is null.");
                }

                if (Convert.ToString(userRoleClaim.Value) != null)
                {
                    _userRole = userRoleClaim.Value;
                    return _userRole.ToLower();
                }
                throw new Exception("User Id can not find");
            }
        }

        public void ChangeCulture(string culture)
        {
            throw new NotImplementedException();
        }

        public void ChangeLanguage(string language)
        {
            throw new NotImplementedException();
        }

        public void SessionAbandon()
        {
            throw new NotImplementedException();
        }

    }
}
