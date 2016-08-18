using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver.Builders;

namespace TeachMe.Models
{
    public class ApplicationUserStore : UserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser, string>, IUserLockoutStore<ApplicationUser, string>
    {
        public ApplicationUserStore() : base()
        {
            
        }

        public ApplicationUserStore(string connectionNameOrUrl) : base(connectionNameOrUrl)
        {
            
        }

        public ApplicationUserStore(string connectionNameOrUrl, string dbName) : base(connectionNameOrUrl, dbName)
        {
            
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            return Task.FromResult(db.GetCollection<ApplicationUser>("AspNetUsers").FindOne(Query.EQ("Email", email)));
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Access.LockoutEndDate);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Access.AccessFailedCount++;
            return Task.FromResult(user.Access.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Access.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Access.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Access.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Access.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            user.Access.LockoutEndDate = lockoutEnd;
            return Task.FromResult(0);
        }
    }
}