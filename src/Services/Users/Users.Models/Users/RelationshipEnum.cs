namespace Lounge.Services.Users.Models.Users
{
    public enum RelationshipEnum
    {
        None = 0,
        IncomingRequest = 1,
        OutgoingRequest = 2,
        Friend = 3,
        Blocked = 4,
        BeingBlocked = 5
    }
}
