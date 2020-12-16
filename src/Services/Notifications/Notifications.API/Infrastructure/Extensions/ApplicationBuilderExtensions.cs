using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.EventHandling;
using Lounge.Services.Notifications.API.IntegrationEvents.Users.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lounge.Services.Notifications.API.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomPathBase(this IApplicationBuilder app, IConfiguration configuration, ILoggerFactory loggerFactory)
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

            eventBus.Subscribe<ConnectionCreatedIntegrationEvent, ConnectionCreatedIntegrationEventHandler>();
            eventBus.Subscribe<ConnectionUpdatedIntegrationEvent, ConnectionUpdatedIntegrationEventHandler>();
            eventBus.Subscribe<GroupRoomMemberAddedIntegrationEvent, GroupRoomMemberAddedIntegrationEventHandler>();
            eventBus.Subscribe<GroupRoomMemberRemovedIntegrationEvent, GroupRoomMemberRemovedIntegrationEventHandler>();
            eventBus.Subscribe<GroupRoomUpdatedIntegrationEvent, GroupRoomUpdatedIntegrationEventHandler>();
            eventBus.Subscribe<RoomCreatedIntegrationEvent, RoomCreatedIntegrationEventHandler>(); 
            eventBus.Subscribe<UserSettingsUpdatedIntegrationEvent, UserSettingsUpdatedIntegrationEventHandler>();
            eventBus.Subscribe<UserUpdatedIntegrationEvent, UserUpdatedIntegrationEventHandler>();

            return app;
        }
    }
}
