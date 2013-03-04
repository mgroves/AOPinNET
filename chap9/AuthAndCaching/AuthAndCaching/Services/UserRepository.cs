using System.Collections.Generic;

namespace AuthAndCaching.Services
{
    public interface IUserRepository
    {
        List<string> GetRolesForCurrentUser();
    }

    public class UserRepository : IUserRepository
    {
        public List<string> GetRolesForCurrentUser()
        {
            return new List<string> {"Manager"};
        }
    }
}