using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Infrastructure.IntegrationEvents;
using Lounge.Services.Users.Models.ConnectionEntities;
using Lounge.Services.Users.Models.RoomEntities;
using Lounge.Services.Users.Services.Rooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lounge.Services.Users.Models.UserEntities;
using Lounge.Services.Users.Services.Rooms.IntegrationEvents;

namespace Lounge.Services.Users.Services.Rooms
{
    public class GroupRoomsService : IGroupRoomsService
    {
        private readonly UsersContext _context;
        private readonly IUsersIntegrationEventService _integrationEventService;

        public GroupRoomsService(
            UsersContext context,
            IUsersIntegrationEventService integrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task<Result> AddRecipientAsync(int roomId, string userToAddId, string callerId)
        {
            var room = await _context.Set<GroupRoom>()
                            .Include(r => r.Members)
                            .ThenInclude(m => m.User)
                            .Where(r => r.Members.Any(c => c.UserId == callerId))
                            .AsNoTracking()
                            .SingleOrDefaultAsync(r => r.Id == roomId);

            if (room is null)
            {
                var error = Errors.GroupRoomNotFound(roomId);
                return Result.Failure(error);
            }

            if (room.Members.Count >= 10)
            {
                var error = Errors.GroupRoomMemberLimitExceeded(roomId);
                return Result.Failure(error);
            }

            var userToAdd = await _context.Set<Connection>()
                .Where(c => c.UserId == callerId)
                .Select(c => new
                {
                    User = new User 
                    {
                        Id = c.OtherId,
                        Name = c.OtherUser.Name, 
                        Tag = c.OtherUser.Tag,
                    },
                    Relationship = c.Relationship
                })
                .SingleOrDefaultAsync(u => u.User.Id == userToAddId);

            if (userToAdd is null)
            {
                var error = Errors.UserNotFound(userToAddId);
                return Result.Failure(error);
            }

            if (userToAdd.Relationship != RelationshipEnum.Friend)
            {
                var error = Errors.UnableToAddNonFriendToRoom(userToAddId);
                return Result.Failure(error);
            }

            var member = new Member
            {
                RoomId = roomId,
                UserId = userToAddId
            };

            _context.Set<Member>().Add(member);
            
            var memberAddedIntegrationEvent = GroupRoomMemberAddedIntegrationEvent.For(room, userToAdd.User);
            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(memberAddedIntegrationEvent);
            await _integrationEventService.PublishThroughEventBusAsync(memberAddedIntegrationEvent);

            return Result.Success;
        }

        public async Task<Result> RemoveRecipientAsync(int roomId, string userToRemoveId, string callerId)
        {
            var room = await _context.Set<GroupRoom>()
                .Include(r => r.Members)
                .Where(r => r.Members.Any(c => c.UserId == callerId))
                .SingleOrDefaultAsync(r => r.Id == roomId);

            if (room is null)
            {
                var error = Errors.GroupRoomNotFound(roomId);
                return Result.Failure(error);
            }

            if (room.OwnerId != callerId)
            {
                var error = Errors.MemberMustBeGroupRoomOwner(roomId, callerId);
                return Result.Failure(error);
            }

            var userToRemove = room.Members
                .FirstOrDefault(cu => cu.UserId == userToRemoveId);

            if (userToRemove is null)
            {
                var error = Errors.MemberNotFound(roomId, userToRemoveId);
                return Result.Failure(error);
            }

            if (userToRemoveId == callerId)
            {
                if (room.Members.Count == 1)
                {
                    _context.Remove(room);

                    var roomDeletedEvent = new GroupRoomDeletedIntegrationEvent(roomId);
                    await _integrationEventService.SaveEventsAndUsersContextChangesAsync(roomDeletedEvent);
                    await _integrationEventService.PublishThroughEventBusAsync(roomDeletedEvent);

                    return Result.Success; 
                }

                room.OwnerId = room.Members
                    .First(cu => cu.UserId != userToRemoveId)
                    .UserId;

                _context.Update(room);
            }

            _context.Remove(userToRemove);

            var memberRemovedEvent = new GroupRoomMemberRemovedIntegrationEvent(roomId, userToRemoveId);
            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(memberRemovedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(memberRemovedEvent);

            return Result.Success;
        }

        public async Task<Result> UpdateAsync(int roomId, GroupRoomUpdateModel model, string callerId)
        {
            var room = await _context.Set<GroupRoom>()
                .Include(r => r.Members)
                .SingleOrDefaultAsync(r => r.Id == roomId);

            if (room is null)
            {
                var error = Errors.GroupRoomNotFound(roomId);
                return Result.Failure(error);
            }

            if (room.OwnerId != model.OwnerId)
            {
                if (room.OwnerId != callerId)
                {
                    var error = Errors.MemberMustBeGroupRoomOwner(roomId, callerId);
                    return Result.Failure(error);
                }

                var newOwnerIsMember = room.Members.Any(r => r.UserId == model.OwnerId);
                if (!newOwnerIsMember)
                {
                    var error = Errors.MemberNotFound(roomId, model.OwnerId);
                    return Result.Failure(error);
                }

                room.OwnerId = model.OwnerId;
            }

            room.Name = model.Name;

            var roomUpdatedEvent = new GroupRoomUpdatedIntegrationEvent(roomId, room.Name, room.OwnerId);
            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(roomUpdatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(roomUpdatedEvent);

            return Result.Success;
        }
    }
}
