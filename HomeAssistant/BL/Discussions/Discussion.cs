using System.Collections.Generic;

namespace BL.Discussions
{
    public class Discussion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual List<Message> Messages { get; set; }

        public Discussion()
        {
            Messages = new List<Message>();
        }

        public void RemoveMessage(Message message)
        {
            Messages.Remove(message);
        }
    }
}