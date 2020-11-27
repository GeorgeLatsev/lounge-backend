using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lounge.BuildingBlocks.EventBus.Abstractions;
using Lounge.BuildingBlocks.EventBus.Events;
using Lounge.BuildingBlocks.IntegrationEventLogEF.Services;
using Lounge.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Lounge.Services.Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lounge.Services.Users.Infrastructure.IntegrationEvents
{
    public class UsersIntegrationEventService : IUsersIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly UsersContext _usersContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<UsersIntegrationEventService> _logger;
        private volatile bool _disposedValue;

        public UsersIntegrationEventService(
            ILogger<UsersIntegrationEventService> logger,
            IEventBus eventBus,
            UsersContext usersContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _usersContext = usersContext ?? throw new ArgumentNullException(nameof(usersContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_usersContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }

        }

        public async Task SaveEventsAndUsersContextChangesAsync(params IntegrationEvent[] events)
        {
            _logger.LogInformation("----- UsersIntegrationEventService - Saving changes and integrationEvents: {IntegrationEventsIds}",
                string.Join(", ", events.Select(evt => evt.Id)));

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_usersContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _usersContext.SaveChangesAsync();
                await _eventLogService.SaveEventsAsync(events, _usersContext.Database.CurrentTransaction);
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    (_eventLogService as IDisposable)?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
