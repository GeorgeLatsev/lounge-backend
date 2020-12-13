using Grpc.Core;
using Lounge.Services.Users.API.Proto;
using Lounge.Services.Users.Models.UserEntities;
using Lounge.Services.Users.Services.Users;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThemeEnum = Lounge.Services.Users.Models.UserEntities.ThemeEnum;

namespace Lounge.Services.Users.API.Grpc
{
    public class UsersGrpcService : UsersGrpc.UsersGrpcBase
    {
        private readonly IUsersService _usersService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<UsersGrpcService> _logger;

        public UsersGrpcService(
            IUsersService usersService,
            ISettingsService settingsService,
            ILogger<UsersGrpcService> logger)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call from method {Method} for user id={Id}", context.Method, request.Id);
            var userId = request.Id;
            var userResult = await _usersService.GetAsync(userId);

            if (!userResult.Succeeded)
            {
                var userNotFound = userResult.Errors.Contains(Errors.UserNotFound(userId));

                if (userNotFound)
                {
                    userResult = await _usersService.CreateAsync(userId);
                }
            }

            if (userResult.Succeeded)
            {
                var settingsResult = await _settingsService.GetAsync(userId);

                if (!settingsResult.Succeeded)
                {
                    var errorStatus = new Status(StatusCode.FailedPrecondition, string.Join(";", settingsResult.Errors));
                    throw new RpcException(errorStatus);
                }

                context.Status = Status.DefaultSuccess;
                return MapToResponse(userResult.Data, settingsResult.Data);
            }

            var status = new Status(StatusCode.FailedPrecondition, string.Join(";", userResult.Errors));
            throw new RpcException(status);
        }

        private static UserResponse MapToResponse(UserModel user, Settings settings)
        {
            var result = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Tag = user.Tag,
                Settings = MapSettings(settings)
            };

            return result;
        }

        private static SettingsResponse MapSettings(Settings settings)
        {
            return new SettingsResponse
            {
                Theme = MapThemeSettingEnum(settings.Theme)
            };

        }

        private static Theme MapThemeSettingEnum(ThemeEnum theme)
        {
            return theme switch
            {
                ThemeEnum.Light => Theme.Light,
                ThemeEnum.Dark => Theme.Dark,
                _ => throw new NotImplementedException()
            };
        }
    }
}

