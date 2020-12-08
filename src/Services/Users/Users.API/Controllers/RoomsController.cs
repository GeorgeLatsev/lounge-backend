using Lounge.Services.Users.API.Infrastructure.Services;
using Lounge.Services.Users.Models.RoomEntities;
using Lounge.Services.Users.Services.Rooms;
using Lounge.Services.Users.Services.Rooms.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lounge.Services.Users.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;
        private readonly IGroupRoomsService _groupRoomsService;
        private readonly IIdentityService _identityService;

        public RoomsController(
            IRoomsService roomsService,
            IGroupRoomsService groupRoomsService,
            IIdentityService identityService)
        {
            _roomsService = roomsService ?? throw new ArgumentNullException(nameof(roomsService));
            _groupRoomsService = groupRoomsService ?? throw new ArgumentNullException(nameof(groupRoomsService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [HttpPost("@me/rooms")]
        public async Task<ActionResult<RoomModel>> CreateRoom([FromBody] string[] othersIds)
        {
            var userId = _identityService.GetUserIdentity();

            var roomResult = await _roomsService.CreateAsync(userId, othersIds);

            if (!roomResult.Succeeded)
            {
                return BadRequest(roomResult.Errors);
            }

            return Ok(roomResult.Data);
        }

        [HttpGet("@me/rooms")]
        public async Task<ActionResult<ICollection<RoomModel>>> GetRooms()
        {
            var userId = _identityService.GetUserIdentity();

            var roomsResult = await _roomsService.GetAllAsync(userId);

            if (!roomsResult.Succeeded)
            {
                return BadRequest(roomsResult.Errors);
            }

            return Ok(roomsResult.Data);
        }

        [HttpPatch("@me/rooms/{roomId}")]
        public async Task<ActionResult> UpdateGroupRoom(int roomId, [FromBody] JsonPatchDocument<GroupRoomUpdateModel> patchDoc)
        {
            var userId = _identityService.GetUserIdentity();

            var roomResult = await _roomsService.GetAsync(roomId);

            if (roomResult.Succeeded)
            {
                return BadRequest(roomResult.Errors);
            }

            var room = roomResult.Data;

            if (room.Type == RoomType.Private)
            {
                var errors = new[] { "Unable to update private room." };
                return BadRequest(errors);
            }

            var groupRoom = room as GroupRoomModel;
            var updateModel = GroupRoomUpdateModel.From(groupRoom);

            patchDoc.ApplyTo(updateModel);

            TryValidateModel(updateModel);
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Keys
                    .SelectMany(k => ModelState[k].Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(validationErrors);
            }

            var updateResult = await _groupRoomsService.UpdateAsync(roomId, updateModel, userId);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }

        [HttpPut("@me/rooms/{roomId}/members/{userToAddId}")]
        public async Task<ActionResult> AddGroupRoomMember(int dmId, string userToAddId)
        {
            var userId = _identityService.GetUserIdentity();
                
            var result = await _groupRoomsService.AddRecipientAsync(dmId, userToAddId, userId);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpDelete("@me/rooms/{roomId}/members/{userToRemoveId}")]
        public async Task<ActionResult> RemoveGroupDmRecipient(int roomId, string userToRemoveId)
        {
            var userId = _identityService.GetUserIdentity();

            var result = await _groupRoomsService.RemoveRecipientAsync(roomId, userToRemoveId, userId);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
