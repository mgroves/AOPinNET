using System;
using AuthAndCaching.Services;

namespace AuthAndCaching.Aspects
{
    public interface IAuthorizationConcern
    {
        void OnEntry(IMethodContextAdapter methodContext, string role);
    }

    public class AuthorizationConcern : IAuthorizationConcern
    {
        readonly IUserRepository _user;

        public AuthorizationConcern(IUserRepository user)
        {
            _user = user;
        }

        public void OnEntry(IMethodContextAdapter methodContext, string role)
        {
            Console.WriteLine("[Auth] Checking if user is in {0} role", role);
            if (UserIsInRole(role))
            {
                Console.WriteLine("[Auth] User IS authorized");
                return;
            }
            Console.WriteLine("[Auth] User is NOT authorized");
            methodContext.AbortMethod();
            UnauthorizedAccess();
        }

        bool UserIsInRole(string role)
        {
            var roles = _user.GetRolesForCurrentUser();
            return roles.Contains(role);
        }

        void UnauthorizedAccess()
        {
            throw new UnauthorizedAccessException("Access denied.");
        }
    }
}