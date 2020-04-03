using System.Collections.ObjectModel;

namespace WebApi.Receiver
{
    public class MultitenancyOptions
    {
        public Collection<AppTenant> Tenants { get; set; }
    }
}
