using NServiceBus.Pipeline;
using System;
using System.Threading.Tasks;

namespace WebApi.Receiver.NServiceBus
{
    public class TenantResolverBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (!context.MessageHeaders.TryGetValue("Tenant", out var tenant))
            {
                throw new Exception("Tenant header is missing!");
            }

            var appTenant = context.Builder.Build<AppTenant>();
            appTenant.Name = tenant;

            return next();

        }
    }
}
