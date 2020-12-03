using Lounge.Services.Users.API.Infrastructure.Services;
using Lounge.Services.Users.Services.Users;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Lounge.Services.Users.Models.UserEntities;

namespace Lounge.Services.Users.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ISettingsService _settingsService;
        private readonly IIdentityService _identityService;

        public UsersController(
            IUsersService usersService,
            ISettingsService settingsService,
            IIdentityService identityService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPatch("@me")]
        public async Task<ActionResult> UpdateUser([FromBody] JsonPatchDocument<UserUpdateModel> patchDoc)
        {
            var userId = _identityService.GetUserIdentity();
            var userResult = await _usersService.GetAsync(userId);

            if (!userResult.Succeeded)
            {
                return BadRequest(userResult.Errors);
            }

            var updateModel = UserUpdateModel.From(userResult.Data);

            patchDoc.ApplyTo(updateModel);
            if (!TryValidateModel(updateModel))
            {
                var validationErrors = ModelState
                    .Keys
                    .SelectMany(k => ModelState[k].Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(validationErrors);
            }

            var updateResult = await _usersService.UpdateAsync(userId, updateModel);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }

        [HttpPatch("@me/settings")]
        public async Task<ActionResult> UpdateSettings([FromBody] JsonPatchDocument<Settings> patchDoc)
        {
            var userId = _identityService.GetUserIdentity();

            var settingsResult = await _settingsService.GetAsync(userId);

            if (!settingsResult.Succeeded)
            {
                return BadRequest(settingsResult.Errors);
            }

            var settings = settingsResult.Data;

            patchDoc.ApplyTo(settings);
            if (!TryValidateModel(settings))
            {
                var validationErrors = ModelState
                    .Keys
                    .SelectMany(k => ModelState[k].Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(validationErrors);
            }

            var updateResult = await _settingsService.UpdateAsync(userId, settings);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }
    }
}
