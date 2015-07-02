using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCalculator.Models
{
    public class Message
    {
        public string Header { get; set; }
        public string Body { get; set; }

        public MessageType Type { get; set; }

        public Message()
        {
            Type = MessageType.Empty;
        }

        public enum MessageType
        {
            Success,
            Info,
            Error,
            Empty
        }
    }
}