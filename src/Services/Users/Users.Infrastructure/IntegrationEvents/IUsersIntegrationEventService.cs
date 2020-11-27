using Lounge.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Lounge.Services.Users.Infrastructure.IntegrationEvents
{
    public interface IUsersIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent evt);

        Task SaveEventsAndUsersContextChangesAsync(params IntegrationEvent[] events);
    }
}
