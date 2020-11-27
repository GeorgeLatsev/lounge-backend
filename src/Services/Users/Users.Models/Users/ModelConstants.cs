using Microsoft.VisualBasic.CompilerServices;

namespace Lounge.Services.Users.Models.Users
{
    public class ModelConstants
    {
        public class User
        {
            public const int MinNameLength = 2;
            public const int MaxNameLength = 24;

            public const int MaxTagLength = 6;
        }

        public class Connection
        {
            public const int MaxNotesLength = 24;
        }
    }
}
