using Lounge.Services.Users.Infrastructure.Data;
using Lounge.Services.Users.Infrastructure.IntegrationEvents;
using Lounge.Services.Users.Models.ConnectionEntities;
using Lounge.Services.Users.Models.RoomEntities;
using Lounge.Services.Users.Models.UserEntities;
using Lounge.Services.Users.Services.Rooms.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lounge.Services.Users.Services.Rooms.IntegrationEvents;

namespace Lounge.Services.Users.Services.Rooms
{
    public class RoomsService : IRoomsService
    {
        private readonly UsersContext _context;
        private readonly IUsersIntegrationEventService _integrationEventService;

        public RoomsService(
            UsersContext context,
            IUsersIntegrationEventService integrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task<Result<RoomModel>> CreateAsync(string userId, string[] othersIds)
        {
            var othersCount = othersIds.Length;
            if (othersCount == 0 || othersCount > 9)
            {
                var error = Errors.InvalidOtherUsersCount(othersCount);
                return Result<RoomModel>.Failure(error);
            }

            var user = await _context.Set<User>()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                var error = Errors.UserNotFound(userId);
                return Result<RoomModel>.Failure(error);
            }

            var otherUsers = await _context.Set<Connection>()
                .Include(u => u.OtherUser)
                .Where(u => u.UserId == userId)
                .Where(u => othersIds.Contains(u.OtherId))
                .Where(u => u.Relationship == RelationshipEnum.Friend)
                .ToListAsync();

            var errors = new List<string>();
            foreach (var otherId in othersIds)
            {
                var otherUserExists = otherUsers.Exists(r => r.OtherId == otherId);

                if (!otherUserExists)
                {
                    var error = Errors.UnableToAddNonFriendToRoom(otherId);
                    errors.Add(error);
                }
            }

            if (errors.Count > 0)
            {
                return Result<RoomModel>.Failure(errors.ToArray());
            }

            Room room;
            if (othersCount < 2)
            {
                var otherId = othersIds.First();
                var roomExists = await _context.Set<Room>()
                    .Where(r => r.Type == RoomType.Private)
                    .Where(r => r.Members.Any(m => m.UserId == userId))
                    .Where(r => r.Members.Any(m => m.UserId == otherId))
                    .AnyAsync();

                if (roomExists)
                {
                    var error = Errors.PrivateRoomAlreadyExists(userId, otherId);
                    return Result<RoomModel>.Failure(error);
                }

                room = new Room();
            }
            else
            {
                room = new GroupRoom()
                {
                    OwnerId = userId
                };
            }

            await _context.AddAsync(room);

            _context.Set<Member>().Add(new Member { RoomId = room.Id, UserId = userId });

            foreach (var other in otherUsers)
            {
                var member = new Member
                {
                    RoomId = room.Id,
                    UserId = other.OtherId
                };

                _context.Set<Member>().Add(member);
            }

            await _context.SaveChangesAsync();

            var roomCreatedEvent = RoomCreatedIntegrationEvent.For(room);
            await _integrationEventService.SaveEventsAndUsersContextChangesAsync(roomCreatedEvent);
            await _integrationEventService.PublishThroughEventBusAsync(roomCreatedEvent);

            var model = RoomModel.MapFrom(room);
            return Result<RoomModel>.SuccessWith(model);

        }

        public async Task<Result<IEnumerable<RoomModel>>> GetAllAsync(string userId)
        {
            var rooms = await _context.Set<Member>()
                .Where(m => m.UserId == userId)
                .Select(m => m.Room)
                .AsNoTracking()
                .Include(r => r.Members)
                .ThenInclude(r => r.User)
                .ToListAsync();

            var models = rooms.Select(RoomModel.MapFrom);
            return Result<IEnumerable<RoomModel>>.SuccessWith(models);
        }

        public async Task<Result<RoomModel>> GetAsync(int id)
        {
            var room = await _context.Set<Room>()
                .AsNoTracking()
                .Include(r => r.Members)
                .ThenInclude(r => r.User)
                .SingleOrDefaultAsync(r => r.Id == id);

            var model = RoomModel.MapFrom(room);
            return Result<RoomModel>.SuccessWith(model);
        }
    }
}
