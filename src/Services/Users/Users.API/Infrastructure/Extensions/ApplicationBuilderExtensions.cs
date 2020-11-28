using Lounge.BuildingBlocks.EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lounge.Services.Users.API.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UserCustomPathBase(this IApplicationBuilder app, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            var pathBase = configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                return app;
            }

            loggerFactory.CreateLogger<Startup>()
                .LogDebug("Using PATH BASE '{pathBase}'", pathBase);
            return app.UsePathBase(pathBase);
        }

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // TODO subscribe IIntegrationEventHandler

            return app;
        }
    }
}
