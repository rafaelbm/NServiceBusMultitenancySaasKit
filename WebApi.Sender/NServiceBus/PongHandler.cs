using Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace WebApi.Sender.NServiceBus
{
    public class PongHandler : IHandleMessages<Pong>
    {
        public Task Handle(Pong message, IMessageHandlerContext context)
        {
            Console.WriteLine("Pong recebido!");

            return Task.CompletedTask;
        }
    }
}
