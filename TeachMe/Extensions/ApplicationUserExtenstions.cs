﻿using TeachMe.Models.Users;

namespace TeachMe.Extensions
{
    public static class ApplicationUserExtenstions
    {
        public static bool IsAdmin(this ApplicationUser user)
        {
            return user.Roles.Contains(UserRole.Admin.Name);
        }
    }
}