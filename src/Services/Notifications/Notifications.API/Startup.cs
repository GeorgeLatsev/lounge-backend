using System.Reflection;
using Autofac;
using HealthChecks.UI.Client;
using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.Hubs;
using Lounge.Services.Notifications.API.Infrastructure.Extensions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling;
using Lounge.Services.Notifications.API.Notifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lounge.Services.Notifications.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc(Configuration)
                .AddCustomSignalR(Configuration)
                .AddCustomRedis(Configuration)
                .AddCustomAuthentication(Configuration)
                .AddCustomIntegrations(Configuration)
                .AddEventBus(Configuration)
                .AddGrpcClients(Configuration)
                .AddHealthChecks(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IPublisherService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(ConnectionCreatedIntegrationEventHandler).Assembly)
                .Where(t => t.IsInstanceOfType(typeof(IIntegrationEventHandler)))
                .AsClosedTypesOf(typeof(IIntegrationEventHandler));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCustomPathBase(Configuration, loggerFactory);

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationsHub>("/notifications");
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });

            app.ConfigureEventBus();
        }
    }
}
