using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace TeachMe.Models
{
    public class UserStore<TUser> : IUserLoginStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserStore<TUser>, IDisposable
        where TUser : IdentityUser
    {
        protected readonly MongoDatabase db;
        private bool _disposed;

        public UserStore()
            : this("DefaultConnection")
        {
        }

        public UserStore(string connectionNameOrUrl)
        {
            if (connectionNameOrUrl.ToLower().StartsWith("mongodb://"))
            {
                db = GetDatabaseFromUrl(new MongoUrl(connectionNameOrUrl));
            }
            else
            {
                var connectionString = ConfigurationManager.ConnectionStrings[connectionNameOrUrl].ConnectionString;
                if (connectionString.ToLower().StartsWith("mongodb://"))
                    db = GetDatabaseFromUrl(new MongoUrl(connectionString));
                else
                    db = GetDatabaseFromSqlStyle(connectionString);
            }
        }

        public UserStore(string connectionNameOrUrl, string dbName)
        {
            if (connectionNameOrUrl.ToLower().StartsWith("mongodb://"))
                db = GetDatabase(connectionNameOrUrl, dbName);
            else
                db = GetDatabase(ConfigurationManager.ConnectionStrings[connectionNameOrUrl].ConnectionString, dbName);
        }

        public UserStore(MongoDatabase mongoDatabase)
        {
            db = mongoDatabase;
        }

        [Obsolete("Use UserStore(connectionNameOrUrl)")]
        public UserStore(string connectionName, bool useMongoUrlFormat)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            if (useMongoUrlFormat)
                db = GetDatabaseFromUrl(new MongoUrl(connectionString));
            else
                db = GetDatabaseFromSqlStyle(connectionString);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (!user.Claims.Any(x =>{
                if (x.ClaimType == claim.Type)
                    return x.ClaimValue == claim.Value;
                return false;
            }))
                user.Claims.Add(new IdentityUserClaim{
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult((IList<Claim>) user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.Claims.RemoveAll(x =>{
                if (x.ClaimType == claim.Type)
                    return x.ClaimValue == claim.Value;
                return false;
            });
            return Task.FromResult(0);
        }

        public Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            db.GetCollection<TUser>("AspNetUsers").Insert(user);
            return Task.FromResult(user);
        }

        public Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            db.GetCollection("AspNetUsers").Remove(Query.EQ("_id", ObjectId.Parse(user.Id)));
            return Task.FromResult(true);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();
            return
                Task.FromResult(db.GetCollection<TUser>("AspNetUsers").FindOne(Query.EQ("_id", ObjectId.Parse(userId))));
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            return Task.FromResult(db.GetCollection<TUser>("AspNetUsers").FindOne(Query.EQ("UserName", userName.ToLowerInvariant())));
        }

        public Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            db.GetCollection<TUser>("AspNetUsers")
              .Update(Query.EQ("_id", ObjectId.Parse(user.Id)), Update.Replace(user), UpdateFlags.Upsert);
            return Task.FromResult(user);
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (!user.Logins.Any(x =>{
                if (x.LoginProvider == login.LoginProvider)
                    return x.ProviderKey == login.ProviderKey;
                return false;
            }))
                user.Logins.Add(login);
            return Task.FromResult(true);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            return
                Task.FromResult(
                    db.GetCollection<TUser>("AspNetUsers")
                      .FindOne(Query.And(Query.EQ("Logins.LoginProvider", login.LoginProvider),
                                         Query.EQ("Logins.ProviderKey", login.ProviderKey))));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult<IList<UserLoginInfo>>(user.Logins.ToList());
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.Logins.RemoveAll(x =>{
                if (x.LoginProvider == login.LoginProvider)
                    return x.ProviderKey == login.ProviderKey;
                return false;
            });
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            if (!user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                user.Roles.Add(role);
            return Task.FromResult(true);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult((IList<string>) user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase));
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.Roles.RemoveAll(r => string.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        private MongoDatabase GetDatabaseFromSqlStyle(string connectionString)
        {
            var builder = new MongoConnectionStringBuilder(connectionString);
            var server = new MongoClient(MongoClientSettings.FromConnectionStringBuilder(builder)).GetServer();
            if (builder.DatabaseName == null)
                throw new Exception("No database name specified in connection string");
            return server.GetDatabase(builder.DatabaseName);
        }

        private MongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var server = new MongoClient(url).GetServer();
            if (url.DatabaseName == null)
                throw new Exception("No database name specified in connection string");
            return server.GetDatabase(url.DatabaseName);
        }

        private MongoDatabase GetDatabase(string connectionString, string dbName)
        {
            return new MongoClient(connectionString).GetServer().GetDatabase(dbName);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}