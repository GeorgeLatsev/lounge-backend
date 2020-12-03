using FluentValidation;
using Lounge.Services.Users.Models.UserEntities;
using Lounge.Services.Users.Services.Users.Models;

namespace Lounge.Services.Users.API.Infrastructure.Validators
{
    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        public UserUpdateModelValidator()
        {
            RuleFor(u => u.Name)
                .Length(ModelConstants.User.MinNameLength, ModelConstants.User.MaxNameLength)
                .Matches("^[A-Za-z0-9-_]*$");
        }
    }
}
