using NServiceBus;
using System;

namespace Messages
{
    public class Ping : IMessage
    {
        public string From { get; set; }
        public DateTime DateTimeNow { get; } = DateTime.Now;
    }
}
