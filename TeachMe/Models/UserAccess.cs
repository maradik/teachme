using System;

namespace TeachMe.Models
{
    public class UserAccess
    {
        public int AccessFailedCount { get; set; }
        public DateTimeOffset LockoutEndDate { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}