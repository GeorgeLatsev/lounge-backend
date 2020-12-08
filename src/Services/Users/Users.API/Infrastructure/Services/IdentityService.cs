using Microsoft.AspNetCore.Http;
using System;

namespace Lounge.Services.Users.API.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserIdentity()
        {
            var claim = _context.HttpContext?.User?.FindFirst("sub");

            if (claim is null)
            {
                throw new InvalidOperationException("Something went wrong while trying to get user identity.");
            }

            return claim.Value;
        }
    }
}
