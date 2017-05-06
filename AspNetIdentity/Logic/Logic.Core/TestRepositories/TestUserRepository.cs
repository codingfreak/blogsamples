namespace codingfreaks.AspNetIdentity.Logic.Core.TestRepositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Core;

    using Extensions;

    using Shared.Enumerations;
    using Shared.Interfaces;
    using Shared.TransportModels;

    /// <summary>
    /// The repository for accessing user data for tests.
    /// </summary>
    public class TestUserRepository : IUserRepository
    {
        #region member vars

        private readonly IRoleRepository _roleRepository;

        private readonly Lazy<List<User>> _store = new Lazy<List<User>>(
            () =>
            {
                var result = new List<User>();
                var lines = File.ReadAllLines(@"Stores\UserStore.txt").ToList();
                lines.ForEach(
                    line =>
                    {
                        var parts = line.Split(';');
                        result.Add(
                            new User
                            {
                                Id = long.Parse(parts[0]),
                                UserName = parts[2],
                                Email = parts[3],
                                PasswordHash = parts[4],
                                EmailConfirmed = bool.Parse(parts[5]),
                                LockoutEnabled = bool.Parse(parts[6])
                            });
                    });
                return result;
            });

        #endregion

        #region constructors and destructors

        public TestUserRepository(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public Task<long?> AddUserAsnyc(UserTransportModel model, string firstRoleName)
        {
            var roleId = _roleRepository.GetRoleIdByNameAsync(firstRoleName).Result;
            if (!roleId.HasValue)
            {
                return null;
            }
            model.Id = _store.Value.Count + 1;
            var line = $"{model.Id};{roleId};{model.UserName};{model.Email};{model.PasswordHash};{model.EmailConfirmed};{model.LockoutEnabled}";
            try
            {
                File.AppendAllLines(@"Stores\UserStore.txt", new[] { line });
            }
            catch (Exception e)
            {
                return Task.FromResult(default(long?));
            }
            return Task.FromResult(model.Id as long?);
        }

        public Task<IEnumerable<string>> GetRoleNamesAsync(long userId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<UserTransportModel> GetUserByIdAsync(long id)
        {
            return Task.FromResult(_store.Value.SingleOrDefault(u => u.Id == id).ToTransportModel());
        }

        /// <inheritdoc />
        public Task<UserTransportModel> GetUserByMailAsync(string mailAddress)
        {
            return Task.FromResult(_store.Value.SingleOrDefault(u => u.Email == mailAddress).ToTransportModel());
        }

        /// <inheritdoc />
        public Task<UserTransportModel> GetUserByUserNameAsync(string userName)
        {
            return Task.FromResult(_store.Value.SingleOrDefault(u => u.UserName == userName).ToTransportModel());
        }

        /// <inheritdoc />
        public Task<IEnumerable<UserTransportModel>> GetUsersAsync()
        {
            return Task.FromResult(_store.Value.Select(u => u.ToTransportModel()).AsEnumerable());
        }

        /// <inheritdoc />
        public Task<PasswordCheckResult> IsPassCorrectAsync(string userName, string passHash)
        {
            var user = GetUserByUserNameAsync(userName).Result;
            if (user == null)
            {
                return Task.FromResult(PasswordCheckResult.UserNotFound);
            }
            if (!user.EmailConfirmed)
            {
                return Task.FromResult(PasswordCheckResult.UserNotConfirmed);
            }
            if (user.LockoutEnabled)
            {
                return Task.FromResult(PasswordCheckResult.UserIsLocked);
            }
            if (!user.PasswordHash.Equals(passHash, StringComparison.Ordinal))
            {
                return Task.FromResult(PasswordCheckResult.PasswordIncorrect);
            }
            return Task.FromResult(PasswordCheckResult.Success);
        }

        /// <inheritdoc />
        public Task<bool> UserExistsAsync(string userName)
        {
            return Task.FromResult(_store.Value.Any(u => u.UserName.Equals(userName, StringComparison.Ordinal)));
        }

        #endregion
    }
}