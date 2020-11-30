using Lounge.Services.Users.API.Infrastructure.Services;
using Lounge.Services.Users.Services.Users;
using Lounge.Services.Users.Services.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lounge.Services.Users.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IIdentityService _identityService;

        public UsersController(
            IUsersService usersService,
            IIdentityService identityService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPatch("@me")]
        public async Task<ActionResult> UpdateCurrentUser([FromBody] JsonPatchDocument<UserUpdateModel> patchDoc)
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
    }
}
