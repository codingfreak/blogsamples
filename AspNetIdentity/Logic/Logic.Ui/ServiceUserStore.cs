using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    using Models;

    /// <summary>
    /// The user store to use for all identity users and types.
    /// </summary>
    public class ServiceUserStore : IUserLoginStore<ApplicationUser, long>,
        IUserClaimStore<ApplicationUser, long>,
        IUserRoleStore<ApplicationUser, long>,
        IUserPasswordStore<ApplicationUser, long>,
        IUserSecurityStampStore<ApplicationUser, long>,
        IQueryableUserStore<ApplicationUser, long>,
        IUserEmailStore<ApplicationUser, long>,
        IUserPhoneNumberStore<ApplicationUser, long>,
        IUserTwoFactorStore<ApplicationUser, long>,
        IUserLockoutStore<ApplicationUser, long>
    {
        #region explicit interfaces

        public void Dispose()
        {
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var user = Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.Ordinal));
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Email);
        }

        /// <inheritdoc />
        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <inheritdoc />
        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <inheritdoc />
        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <inheritdoc />
        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEndDateUtc ?? DateTimeOffset.MaxValue);
        }

        /// <inheritdoc />
        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() => user.LockoutEnabled = enabled);
        }

        /// <inheritdoc />
        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <inheritdoc />
        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        /// <inheritdoc />
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public async Task CreateAsync(ApplicationUser user)
        {
            if (user.Id != 0)
            {
                return;
            }
            user.Id = await ApiClient.CreateUserAsync(user) ?? 0;
        }

        /// <inheritdoc />
        public async Task DeleteAsync(ApplicationUser user)
        {
            await ApiClient.DeleteUserAsync(user);
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            var user = Users.SingleOrDefault(u => u.Id == userId);
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = Users.SingleOrDefault(u => u.UserName.Equals(userName, StringComparison.Ordinal));
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(ApplicationUser user)
        {
            await ApiClient.UpdateUserAsync(user);
        }

        #endregion

        #region methods

        /// <inheritdoc />
        public Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return Task.FromResult<IList<Claim>>(user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList());
        }

        /// <inheritdoc />
        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <inheritdoc />
        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <inheritdoc />
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return user.RolesResolved ?? (user.RolesResolved = await ApiClient.GetUserRoleNamesAsync(user.Id));
        }

        /// <inheritdoc />
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        /// <inheritdoc />
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <inheritdoc />
        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            var roles = await GetRolesAsync(user);
            return roles.Contains(roleName);
        }

        /// <inheritdoc />
        public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            foreach (var item in user.Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList())
            {
                user.Claims.Remove(item);
            }
            // TODO remove all claims on Service
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        /// <inheritdoc />
        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        #region properties

        /// <inheritdoc />
        public IQueryable<ApplicationUser> Users
        {
            get
            {
                var result = Task.Run(async () => await ApiClient.GetUsersAsync()).GetAwaiter().GetResult().AsQueryable();
                return result;
            }
        }

        #endregion
    }
}