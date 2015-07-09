using System.Linq;

namespace BL.Users
{
    public interface IUserReader
    {
        IQueryable<User> Users { get; }
        User this[string login] { get; }
    }
}
