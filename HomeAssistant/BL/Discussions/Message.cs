using System;
using BL.Users;

namespace BL.Discussions
{
    public class Message
    {
        public int Id { get; set; }
        public virtual User Author { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
