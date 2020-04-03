using Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace WebApi.Receiver.NServiceBus
{
    public class PingHandler : IHandleMessages<Ping>
    {
        private readonly AppTenant _appTenant;

        public PingHandler(AppTenant appTenant)
        {
            _appTenant = appTenant;
        }

        public async Task Handle(Ping message, IMessageHandlerContext context)
        {
            Console.WriteLine(_appTenant);

            Console.WriteLine("Recebido ping...");

            await context.Reply(new Pong { From = "WebApi.Receiver" });

            Console.WriteLine("Respondido com pong...");
        }
    }
}
