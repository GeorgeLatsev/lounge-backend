using Lounge.Services.Users.Services.Connections.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Infrastructure.IntegrationEvents;
using Lounge.Services.Users.Models.Users;
using Lounge.Services.Users.Services.Connections.IntegrationEvents;
using Lounge.Services.Users.Services.Users;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.EntityFrameworkCore;
using static Lounge.Services.Users.Models.Users.RelationshipEnum;

namespace Lounge.Services.Users.Services.Connections
{
    public class ConnectionsService : IConnectionsService
    {
        private readonly UsersContext _context;
        private readonly IUsersIntegrationEventService _integrationEventService;

        public ConnectionsService(
            UsersContext context,
            IUsersIntegrationEventService integrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task<Result<IEnumerable<ConnectionModel>>> GetAllAsync(string userId)
        {
            var connections = await _context.Set<Connection>()
                .Include(c => c.OtherUser)
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Where(c => c.Relationship != RelationshipEnum.None || c.Notes != string.Empty)
                .ToListAsync();

            var models = connections.Select(ConnectionModel.MapFrom);
            return Result<IEnumerable<ConnectionModel>>.SuccessWith(models);
        }

        public async Task<Result<ConnectionModel>> GetAsync(string userId, string otherId)
        {
            if (userId == otherId)
            {
                var error = ConnectionsServiceErrors.UnableToGetConnectionToSelf();
                return Result<ConnectionModel>.Failure(error);
            }

            var connection = await _context.Set<Connection>()
                .AsNoTracking()
                .Include(c => c.OtherUser)
                .SingleOrDefaultAsync(c => c.UserId == userId && c.OtherUserId == otherId);

            if (connection is null)
            {
                var userExists = await _context.Set<User>().AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    var error = ConnectionsServiceErrors.UserNotFound(userId);
                    return Result<ConnectionModel>.Failure(error);
                }

                var other = await _context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.Id == otherId);

                if (other is null)
                {
                    var error = ConnectionsServiceErrors.UserNotFound(otherId);
                    return Result<ConnectionModel>.Failure(error);
                }

                connection = new Connection { UserId =  userId, OtherUserId = otherId };

                _context.Set<Connection>().Add(connection);

                var connectionCreatedEvent = new ConnectionCreatedIntegrationEvent(userId, otherId, other.Name, other.Tag);

                await _integrationEventService.SaveEventsAndUsersContextChangesAsync(connectionCreatedEvent);
                await _integrationEventService.PublishThroughEventBusAsync(connectionCreatedEvent);

                connection.OtherUser = other;
            }

            var model = ConnectionModel.MapFrom(connection);
            return Result<ConnectionModel>.SuccessWith(model);
        }

        public async Task<Result<ConnectionModel>> GetByDisplayNameAsync(string userId, string displayName)
        {
            var other = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => displayName == $"{u.Name}#{u.Tag}");

            if (other is null)
            {
                var error = ConnectionsServiceErrors.UserWithDisplayNameNotFound(displayName);
                return Result<ConnectionModel>.Failure(error);
            }

            if (userId == other.Id)
            {
                var error = ConnectionsServiceErrors.UnableToGetConnectionToSelf();
                return Result<ConnectionModel>.Failure(error);
            }

            var connection = await _context.Set<Connection>()
                .AsNoTracking()
                .Include(c => c.OtherUser)
                .SingleOrDefaultAsync(c => c.UserId == userId && c.OtherUserId == other.Id);

            if (connection is null)
            {
                var userExists = await _context.Set<User>().AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    var error = ConnectionsServiceErrors.UserNotFound(userId);
                    return Result<ConnectionModel>.Failure(error);
                }

                connection = new Connection { UserId = userId, OtherUserId = other.Id };

                _context.Set<Connection>().Add(connection);

                var connectionCreatedEvent = new ConnectionCreatedIntegrationEvent(userId, other.Id, other.Name, other.Tag);

                await _integrationEventService.SaveEventsAndUsersContextChangesAsync(connectionCreatedEvent);
                await _integrationEventService.PublishThroughEventBusAsync(connectionCreatedEvent);

                connection.OtherUser = other;
            }

            var model = ConnectionModel.MapFrom(connection);
            return Result<ConnectionModel>.SuccessWith(model);
        }

        public async Task<Result> UpdateAsync(string userId, string otherId, ConnectionUpdateModel model)
        {
            var connectionResult = await GetAsync(userId, otherId);

            if (!connectionResult.Succeeded)
            {
                return Result.Failure(connectionResult.Errors);
            }

            var connection = connectionResult.Data;

            connection.Notes = model.Notes;

            if (connection.Relationship == model.Relationship)
            {
                _context.Update(connection);

                var connectionUpdatedEvent = new ConnectionUpdatedntegrationEvent(userId, otherId, connection.Notes, connection.Relationship);

                await _integrationEventService.SaveEventsAndUsersContextChangesAsync(connectionUpdatedEvent);
                await _integrationEventService.PublishThroughEventBusAsync(connectionUpdatedEvent);

                return Result.Success;
            }

            var otherResult = await GetAsync(otherId, userId);

            if (!otherResult.Succeeded)
            {
                return Result.Failure(otherResult.Errors);
            }

            var other = otherResult.Data;

            RelationshipEnum? relationshipForOther = null;

            switch (connection.Relationship) // TODO refactor or document
            {
                case None:
                {
                    if (model.Relationship == OutgoingRequest)
                    {
                        relationshipForOther = IncomingRequest;
                    }

                    if (model.Relationship == Blocked)
                    {
                        relationshipForOther = BeingBlocked;
                    }

                    break;
                }

                case IncomingRequest:
                {
                    if (model.Relationship == Friend || model.Relationship == OutgoingRequest)
                    {
                        relationshipForOther = Friend;
                        model.Relationship = Friend;
                    }

                    if (model.Relationship == Blocked)
                    {
                        relationshipForOther = BeingBlocked;
                    }

                    break;
                }

                case OutgoingRequest:
                case Friend:
                {
                    if (model.Relationship == None)
                    {
                        relationshipForOther = None;
                    }

                    if (model.Relationship == Blocked)
                    {
                        relationshipForOther = BeingBlocked;
                    }

                    break;
                }

                case RelationshipEnum.Blocked:
                {
                    if (model.Relationship == None)
                    {
                        if (other.Relationship == Blocked)
                        {
                            model.Relationship = BeingBlocked;
                        }
                        else
                        {
                            other.Relationship = None;
                        }
                    }

                    break;
                }

                case RelationshipEnum.BeingBlocked:
                {
                    if (model.Relationship == Blocked)
                    {
                        other.Relationship = Blocked;
                    }

                    break;
                }
            }

            if (!relationshipForOther.HasValue)
            {
                var error = ConnectionsServiceErrors.InvalidRelationshipUpdate();
                return Result.Failure(error);
            }

            connection.Relationship = model.Relationship;
            _context.Update(connection);

            other.Relationship = relationshipForOther.Value;
            _context.Update(other);

            var userConnectionUpdatedEvent = new ConnectionUpdatedntegrationEvent(userId, otherId, connection.Notes, connection.Relationship);
            var otherConnectionUpdatedEvent = new ConnectionUpdatedntegrationEvent(otherId, userId, other.Notes, other.Relationship);

            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(userConnectionUpdatedEvent, otherConnectionUpdatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(userConnectionUpdatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(otherConnectionUpdatedEvent);

            return Result.Success;
        }
    }
}
