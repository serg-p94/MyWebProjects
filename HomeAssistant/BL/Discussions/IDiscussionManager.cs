using System.Linq;

namespace BL.Discussions
{
    public interface IDiscussionManager
    {
        void CreateDiscussion(string tilte);
        IQueryable<Discussion> Discussions { get; }
        void Remove(Discussion discussion);
        void Update();
    }
}
