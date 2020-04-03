using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Receiver
{
    public class AppTenantResolver : ITenantResolver<AppTenant>
    {
        private readonly IEnumerable<AppTenant> _availableTenants;

        public AppTenantResolver(IOptions<MultitenancyOptions> options)
        {
            _availableTenants = options.Value.Tenants;
        }

        public Task<TenantContext<AppTenant>> ResolveAsync(HttpContext context)
        {
            TenantContext<AppTenant> tenantContext = null;

            var tenant = _availableTenants.FirstOrDefault(t =>
                t.Hostnames.Any(hostname =>
                    hostname.Equals(context.Request.Host.ToString(), StringComparison.OrdinalIgnoreCase)));

            if (tenant != null)
            {
                tenantContext = new TenantContext<AppTenant>(tenant);
            }

            return Task.FromResult(tenantContext);
        }
    }
}
