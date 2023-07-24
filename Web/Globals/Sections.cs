﻿using Common.Options;

namespace Web.Globals
{
    public static class Sections
    {

        public static JwtOption JwtOption { get; set; }
        public static ImageOption ImageOption { get; set; }
        public static AppInfoOption AppInfoOption { get; set; }
        public static ConsulOptions ConsulOptions { get; set; }
        public static PersistenceConfiguration Persistence { get; set; }
        public const string Password = "PasswordOption";
        public const string IdentityLockout = "IdentityLockoutOption";
    }
}