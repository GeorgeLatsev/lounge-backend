using Lounge.Services.Users.API.Grpc.Clients.Notifications;
using Lounge.Services.Users.API.Infrastructure.Services;
using Lounge.Services.Users.Models.ConnectionEntities;
using Lounge.Services.Users.Services.Connections;
using Lounge.Services.Users.Services.Connections.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lounge.Services.Users.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly IConnectionsService _connectionsService;
        private readonly IIdentityService _identityService;
        private readonly INotificationsGrpcService _notificationsGrpcService;

        public ConnectionsController(
            IConnectionsService connectionsService,
            IIdentityService identityService,
            INotificationsGrpcService notificationsGrpcService)
        {
            _connectionsService = connectionsService ?? throw new ArgumentNullException(nameof(connectionsService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _notificationsGrpcService = notificationsGrpcService ?? throw new ArgumentNullException(nameof(notificationsGrpcService));
        }

        [HttpGet("{displayName}")]
        public async Task<ActionResult<ConnectionModel>> GetConnection(
            string displayName,
            [FromHeader(Name = "notifications-connection-id")][Required] string connectionId)
        {
            var userId = _identityService.GetUserIdentity();

            var connectionResult = await _connectionsService.GetByDisplayNameAsync(userId, displayName);

            if (!connectionResult.Succeeded)
            {
                return BadRequest(connectionResult.Errors);
            }

            // subscribe
            var otherId = connectionResult.Data.OtherId;
            await _notificationsGrpcService.SubscribeToUsersUpdatesAsync(userId, connectionId, otherId);

            // return
            return Ok(connectionResult.Data);
        }

        [HttpGet("@me/connections")]
        public async Task<ActionResult<ICollection<ConnectionModel>>> GetConnections(
            [FromHeader(Name = "notifications-connection-id")][Required] string connectionId)
        {
            var userId = _identityService.GetUserIdentity();

            var connectionsResult = await _connectionsService.GetAllAsync(userId);

            if (!connectionsResult.Succeeded)
            {
                return BadRequest(connectionsResult.Errors);
            }

            // subscribe
            var usersIds = connectionsResult.Data
                .Select(ur => ur.OtherId)
                .ToArray();

            await _notificationsGrpcService.SubscribeToUsersUpdatesAsync(userId, connectionId, usersIds);

            var friendsIds = connectionsResult.Data
                .Where(ur => ur.Relationship == RelationshipEnum.Friend)
                .Select(ur => ur.OtherId)
                .ToArray();

            await _notificationsGrpcService.SubscribeToUsersStatusUpdatesAsync(userId, connectionId, friendsIds);

            // return
            return Ok(connectionsResult.Data);
        }

        [HttpPatch("@me/connections/{otherId}")]
        public async Task<ActionResult> UpdateConnection(string otherId, [FromBody] JsonPatchDocument<ConnectionUpdateModel> patchDoc)
        {
            var userId = _identityService.GetUserIdentity();

            var connectionResult = await _connectionsService.GetAsync(userId, otherId);

            if (!connectionResult.Succeeded)
            {
                return BadRequest(connectionResult.Errors);
            }

            var existingConnection = connectionResult.Data;
            var updateModel = ConnectionUpdateModel.From(existingConnection);

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

            var updateResult = await _connectionsService.UpdateAsync(userId, otherId, updateModel);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok();
        }
    }
}
