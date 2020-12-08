namespace Lounge.Services.Users.Services.Rooms
{
    public static class Errors
    {
        public static string GroupRoomMemberLimitExceeded(int roomId) => $"Group room member limit exceeded. id={roomId}";
        
        public static string GroupRoomNotFound(int roomId) => $"Group room not found. id={roomId}";
        
        public static string InvalidOtherUsersCount(int othersCount) => $"Unable to create room due to invalid other users count. othersCount={othersCount}";
        
        public static string MemberMustBeGroupRoomOwner(int roomId, string userId) => $"Member must be group room owner. roomId={roomId} memberId={userId}";
        
        public static string MemberNotFound(int roomId, string userId) => $"Member not found. roomId={roomId} memberId={userId}";
        
        public static string PrivateRoomAlreadyExists(string userId, string otherId) => $"Private room already exists. users={userId},{otherId}";
        
        public static string UnableToAddNonFriendToRoom(string otherId) => $"Unable to add non-friend to room. otherId={otherId}";
        
        public static string UserNotFound(string id) => $"User not found. id={id}";
    }
}