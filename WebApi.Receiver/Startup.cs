using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SaasKit.Multitenancy;
using SaasKit.Multitenancy.Internal;
using System;

namespace WebApi.Receiver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddMultitenancy<AppTenant, AppTenantResolver>();
            services.MyAddMultitenancy<AppTenant, AppTenantResolver>();

            services.Configure<MultitenancyOptions>(Configuration.GetSection("Multitenancy"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMultitenancy<AppTenant>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class MultitenancyServiceCollectionExtensions
    {
        public static IServiceCollection MyAddMultitenancy<TTenant, TResolver>(this IServiceCollection services)
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class, new()
        {
            Ensure.Argument.NotNull(services, nameof(services));

            services.AddScoped<ITenantResolver<TTenant>, TResolver>();

            // No longer registered by default as of ASP.NET Core RC2
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Make Tenant and TenantContext injectable
            services.AddScoped(prov =>
            {
                var tenantContext = prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>();

                if (tenantContext != null)
                    return tenantContext;

                var tenant = Activator.CreateInstance<TTenant>();
                
                // Uso essa forma diferente de criar instâncias através de reflection para poder passar o Tenant criado
                // para o construtor de TenantContext<TTenant>
                tenantContext = (TenantContext<TTenant>)Activator.CreateInstance(typeof(TenantContext<TTenant>), tenant);

                return tenantContext;
            });

            services.AddScoped(prov => prov.GetService<TenantContext<TTenant>>()?.Tenant);

            // Make ITenant injectable for handling null injection, similar to IOptions
            services.AddScoped<ITenant<TTenant>>(prov => new TenantWrapper<TTenant>(prov.GetService<TTenant>()));

            // Ensure caching is available for caching resolvers
            var resolverType = typeof(TResolver);
            if (typeof(MemoryCacheTenantResolver<TTenant>).IsAssignableFrom(resolverType))
            {
                services.AddMemoryCache();
            }

            return services;
        }
    }
}
