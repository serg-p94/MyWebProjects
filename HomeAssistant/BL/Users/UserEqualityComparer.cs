using System.Collections.Generic;

namespace BL.Users
{
    public class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.Login == y.Login;
        }

        public int GetHashCode(User obj)
        {
            return obj.Login.GetHashCode();
        }
    }
}
