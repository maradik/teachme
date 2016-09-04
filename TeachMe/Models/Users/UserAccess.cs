using System;

namespace TeachMe.Models.Users
{
    public class UserAccess
    {
        public int AccessFailedCount { get; set; }
        public DateTimeOffset LockoutEndDate { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}