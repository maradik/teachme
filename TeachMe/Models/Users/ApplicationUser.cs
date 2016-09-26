using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using System.Collections.Generic;

namespace TeachMe.Models.Users
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        private UserAccess access;
        private string userName;
        private UserCash cash;
        private List<int> subjectIds;

        public override string UserName { get { return userName; } set { userName = value.ToLowerInvariant(); } }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public UserAccess Access { get { return access ?? (access = new UserAccess()); } set { access = value; } }
        public UserCash Cash { get { return cash ?? (cash = new UserCash()); } set { cash = value; } }
        public List<int> SubjectIds { get { return subjectIds ?? (subjectIds = new List<int>()); } set { subjectIds = value; } }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
}