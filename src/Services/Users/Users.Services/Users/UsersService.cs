using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Infrastructure.IntegrationEvents;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Lounge.Services.Users.Models.UserEntities;
using Lounge.Services.Users.Services.Users.IntegrationEvents;

namespace Lounge.Services.Users.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UsersContext _context;
        private readonly IUsersIntegrationEventService _integrationEventService;
        private readonly ITagGeneratorService _tagGeneratorService;

        public UsersService(
            UsersContext context,
            IUsersIntegrationEventService integrationEventService, 
            ITagGeneratorService tagGeneratorService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
            _tagGeneratorService = tagGeneratorService ?? throw new ArgumentNullException(nameof(tagGeneratorService));
        }

        public async Task<Result<UserModel>> CreateAsync(string id)
        {
            var alreadyExists = await _context.Set<User>()
                .AnyAsync(u => u.Id == id);

            if (alreadyExists)
            {
                var error = Errors.UserAlreadyExists(id);
                return Result<UserModel>.Failure(error);
            }

            var user = new User { Id = id };

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            var model = UserModel.MapFrom(user);
            return Result<UserModel>.SuccessWith(model);
        }

        public async Task<Result<UserModel>> GetAsync(string id)
        {
            var user = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Settings)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                var error = Errors.UserNotFound(id);
                return Result<UserModel>.Failure(error);
            }

            var model = UserModel.MapFrom(user);
            return Result<UserModel>.SuccessWith(model);
        }

        public async Task<Result> UpdateAsync(string id, UserUpdateModel model)
        {
            var user = await _context.Set<User>()
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                var error = Errors.UserNotFound(id);
                return Result.Failure(error);
            }

            if (user.Name != model.Name)
            {
                user.Name = model.Name;

                var tagResult = await _tagGeneratorService.GenerateAsync(id, model.Name);

                if (!tagResult.Succeeded)
                {
                    return Result.Failure(tagResult.Errors);
                }

                user.Tag = tagResult.Data;
            }

            var userUpdatedEvent = new UserUpdatedIntegrationEvent(id, user.Name, user.Tag);

            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(userUpdatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(userUpdatedEvent);

            return Result.Success;
        }
    }
}
