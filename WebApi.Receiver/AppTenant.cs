using System.Collections.Generic;

namespace WebApi.Receiver
{
    public class AppTenant
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Hostnames { get; set; } = new List<string>();
        public string Theme { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Name=${Name};Hostnames=${string.Join(", ", Hostnames)};Theme=${Theme};ConnectionString=${ConnectionString}";
        }
    }
}
