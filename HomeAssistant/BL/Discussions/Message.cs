using BL.Users;
using System;

namespace BL.Discussions
{
    public class Message
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
