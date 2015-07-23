using System.Configuration;
using System.Linq;
using BL.Discussions;

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

        public IQueryable<Discussion> Discussions
        {
            get { return _context.Discussions; }
        }

        public void Remove(Discussion discussion)
        {
            _context.Discussions.Remove(discussion);
            _context.SaveChanges();
        }

        public void Update()
        {
            _context.SaveChanges();
        }
    }
}
