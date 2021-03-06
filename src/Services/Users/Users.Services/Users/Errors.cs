﻿namespace Lounge.Services.Users.Services.Users
{
    public static class Errors
    {
        public static string NameOccurrencesLimitExceeded(string name) => $"Too many users already have that name. name={name}";

        public static string UserAlreadyExists(string id) =>  $"User already exists. id={id}";

        public static string UserNotFound(string id) => $"User not found. id={id}";
    }
}