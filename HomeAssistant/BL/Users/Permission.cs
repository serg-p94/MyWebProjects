using System.Collections.Generic;

namespace BL.Users
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual HashSet<User> Users { get; protected set; }

        public Permission()
        {
            Users = new HashSet<User>();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Id, Name);
        }
    }
}
