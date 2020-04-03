using NServiceBus;
using System;

namespace Messages
{
    public class Pong : IMessage
    {
        public string From { get; set; }
        public DateTime DateTimeNow { get; } = DateTime.Now;
    }
}
