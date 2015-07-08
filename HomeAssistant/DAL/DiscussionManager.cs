using BL.Discussions;
using System.Configuration;
using System.Linq;

namespace DAL
{
    public class DiscussionManager : IDiscussionManager
    {
        private readonly MainDbContext _context;
        public static readonly string NameOrConnectionString = ConfigurationManager.ConnectionStrings["MainDbContext"].ConnectionString;

        public DiscussionManager()
        {
            _context = new MainDbContext(NameOrConnectionString);
        }

        public void CreateDiscussion(string tilte)
        {
            _context.Discussions.Add(new Discussion { Title = tilte });
            _context.SaveChanges();
        }

        public IQueryable<Discussion> GetDiscussions()
        {
            return _context.Discussions;
        }

        public void Update()
        {
            _context.SaveChanges();
        }
    }
}
