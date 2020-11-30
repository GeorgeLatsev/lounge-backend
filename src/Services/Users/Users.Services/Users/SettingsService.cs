using Lounge.Services.Users.Models.Users;
using System;
using System.Threading.Tasks;
using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Infrastructure.IntegrationEvents;
using Lounge.Services.Users.Services.Users.IntegrationEvents;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Lounge.Services.Users.Services.Users
{
    public class SettingsService : ISettingsService
    {
        private readonly UsersContext _context;
        private readonly IUsersIntegrationEventService _integrationEventService;

        public SettingsService(
            UsersContext context,
            IUsersIntegrationEventService integrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task<Result<Settings>> GetAsync(string userId)
        {
            var user = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Settings)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                var error = UsersServiceErrors.UserNotFound(userId);
                return Result<Settings>.Failure(error);
            }

            return Result<Settings>.SuccessWith(user.Settings);
        }

        public async Task<Result> UpdateAsync(string userId, Settings settings)
        {
            var user = await _context.Set<User>()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                var error = UsersServiceErrors.UserNotFound(userId);
                return Result.Failure(error);
            }

            user.Settings = settings;

            var settingsUpdatedEvent = new UserSettingsUpdatedIntegrationEvent(userId, settings.Theme);

            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(settingsUpdatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(settingsUpdatedEvent);

            return Result.Success;
        }
    }
}
