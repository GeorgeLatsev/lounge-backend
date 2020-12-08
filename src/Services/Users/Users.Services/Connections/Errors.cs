namespace Lounge.Services.Users.Services.Connections
{
    public static class Errors
    {
        public static string InvalidRelationshipUpdate() => "Invalid relationship update.";

        public static string UnableToGetConnectionToSelf() => "Unable to get connection to self.";
        
        public static string UserNotFound(string id) => $"User not found. id={id}";

        public static string UserWithDisplayNameNotFound(string displayName) => $"User not found. displayName={displayName}";
    }
}