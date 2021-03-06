﻿using FluentValidation;
using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.API.Infrastructure.Validators
{
    public class SettingsValidator : AbstractValidator<Settings>
    {
        public SettingsValidator()
        {
            RuleFor(u => u.Theme)
                .IsInEnum();
        }
    }
}
