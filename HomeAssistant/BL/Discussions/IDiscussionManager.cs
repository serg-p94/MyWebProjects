using System.Collections.Generic;
using System.Linq;

namespace BL.Discussions
{
    public interface IDiscussionManager
    {
        void CreateDiscussion(string tilte);
        IQueryable<Discussion> GetDiscussions();
        void Update();
    }
}
