using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    using Models;

    /// <summary>
    /// The user store to use for all identity users and types.
    /// </summary>
    public class ServiceUserStore : IUserLoginStore<ApplicationUser, long>, IUserLockoutStore<ApplicationUser, long>
    {
        #region explicit interfaces

        public void Dispose()
        {
        }

        /// <inheritdoc />
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
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
        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}