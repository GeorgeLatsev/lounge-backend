using FluentValidation;
using Lounge.Services.Users.Models.RoomEntities;
using Lounge.Services.Users.Services.Rooms.Models;

namespace Lounge.Services.Users.API.Infrastructure.Validators
{
    public class GroupRoomUpdateModelValidator : AbstractValidator<GroupRoomUpdateModel>
    {
        public GroupRoomUpdateModelValidator()
        {
            RuleFor(r => r.Name)
                .Length(ModelConstants.GroupRoom.MinNameLength, ModelConstants.GroupRoom.MaxNameLength)
                .Matches(@"^[A-Za-z0-9-_ \(\)\[\]]*$");

            RuleFor(r => r.OwnerId)
                .NotNull();
        }
    }
}
