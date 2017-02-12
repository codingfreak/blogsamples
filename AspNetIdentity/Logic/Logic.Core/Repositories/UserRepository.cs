using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codingfreaks.AspNetIdentity.Logic.Core.Repositories
{
    using System.Data.Entity;

    using Data.Core;

    using Extensions;

    using Shared.Enumerations;
    using Shared.Interfaces;
    using Shared.TransportModels;

    /// <summary>
    /// The repository for accessing user data.
    /// </summary>
    public class UserRepository : BaseRespository, IUserRepository
    {
        #region member vars

        private readonly IRoleRepository _roleRepository;

        #endregion

        #region constructors and destructors

        public UserRepository(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async Task<long?> AddUserAsnyc(UserTransportModel model, string firstRoleName)
        {
            var roleId = await _roleRepository.GetRoleIdByNameAsync(firstRoleName);
            if (!roleId.HasValue)
            {
                HandleException(new ArgumentException("Role not found.", nameof(firstRoleName)));
                return null;
            }
            var newUser = model.ToEntity();
            newUser.EmailConfirmed = true;
            newUser.AccessFailedCount = 0;
            newUser.LockoutEnabled = false;
            newUser.LockoutEndDateUtc = null;
            var userRole = new UserRole
            {
                RoleId = roleId.Value
            };
            newUser.UserRoles.Add(userRole);
            DbContext.Users.Add(newUser);
            try
            {
                await DbContext.SaveChangesAsync();
                if (newUser.Id == 0)
                {
                    await DbContext.Entry(newUser).ReloadAsync();
                }
                return newUser.Id;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return default(long?);
            }
        }

        /// <inheritdoc />
        public async Task<UserTransportModel> GetUserByIdAsync(long id)
        {
            var user = await DbContext.Users.FindAsync(id).ConfigureAwait(false);
            return user?.ToTransportModel();
        }

        /// <inheritdoc />
        public async Task<UserTransportModel> GetUserByMailAsync(string mailAddress)
        {
            var user = await DbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(mailAddress, StringComparison.Ordinal)).ConfigureAwait(false);
            return user?.ToTransportModel();
        }

        /// <inheritdoc />
        public async Task<UserTransportModel> GetUserByUserNameAsync(string userName)
        {
            var user = await DbContext.Users.SingleOrDefaultAsync(u => u.UserName.Equals(userName, StringComparison.Ordinal)).ConfigureAwait(false);
            return user?.ToTransportModel();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserTransportModel>> GetUsersAsync()
        {
            var users = await DbContext.Users.ToListAsync().ConfigureAwait(false);
            return users.Select(u => u.ToTransportModel()).ToList();
        }

        /// <inheritdoc />
        public async Task<PasswordCheckResult> IsPassCorrectAsync(string userName, string passHash)
        {
            var user = await GetUserByUserNameAsync(userName).ConfigureAwait(false);
            if (user == null)
            {
                return PasswordCheckResult.UserNotFound;
            }
            if (!user.EmailConfirmed)
            {
                return PasswordCheckResult.UserNotConfirmed;
            }
            if (user.LockoutEnabled)
            {
                return PasswordCheckResult.UserIsLocked;
            }
            if (!user.PasswordHash.Equals(passHash, StringComparison.Ordinal))
            {
                return PasswordCheckResult.PasswordIncorrect;
            }
            return PasswordCheckResult.Success;
        }

        /// <inheritdoc />
        public async Task<bool> UserExistsAsync(string userName)
        {
            return await DbContext.Users.AnyAsync(u => u.UserName.Equals(userName, StringComparison.Ordinal)).ConfigureAwait(false);
        }

        #endregion
    }
}
