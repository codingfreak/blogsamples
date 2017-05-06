namespace codingfreaks.AspNetIdentity.Logic.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Enumerations;

    using TransportModels;

    /// <summary>
    /// Must be implemented by all repositories that are dealing with user data.
    /// </summary>
    public interface IUserRepository
    {
        #region methods

        /// <summary>
        /// Creates a new user using the provided <paramref name="model" />.
        /// </summary>
        /// <param name="model">The model containing the user informations.</param>
        /// <param name="firstRoleName">The name of the first role for this user.</param>
        /// <returns>The database id of the new user or <c>null</c> if the operation fails.</returns>
        Task<long?> AddUserAsnyc(UserTransportModel model, string firstRoleName);

        /// <summary>
        /// Retrieves a list of all role names the user is member of.
        /// </summary>
        /// <param name="userId">The database id of the user.</param>
        /// <returns>The list of role names.</returns>
        Task<IEnumerable<string>> GetRoleNamesAsync(long userId);

        /// <summary>
        /// Retrieves a single user by searching for a given <paramref name="id" />.
        /// </summary>
        /// <param name="id">The database id of the user.</param>
        /// <returns>The user information or <c>null</c> if the user wasn't found.</returns>
        Task<UserTransportModel> GetUserByIdAsync(long id);

        /// <summary>
        /// Retrieves a single user by searching for a given <paramref name="mailAddress" />.
        /// </summary>
        /// <param name="mailAddress">The mail address of the user.</param>
        /// <returns>The user information or <c>null</c> if the user wasn't found.</returns>
        Task<UserTransportModel> GetUserByMailAsync(string mailAddress);

        /// <summary>
        /// Retrieves a single user by searching for a given <paramref name="userName" />.
        /// </summary>
        /// <param name="userName">The user name of the user.</param>
        /// <returns>The user information or <c>null</c> if the user wasn't found.</returns>
        Task<UserTransportModel> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Retrieves a list of all users currently stored in the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        Task<IEnumerable<UserTransportModel>> GetUsersAsync();

        /// <summary>
        /// Checks if a given <paramref name="userName" />-<paramref name="passHash" />-combination is valid.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <param name="passHash">The password hash to use for the check.</param>
        /// <returns>An enum showing the result of the operation.</returns>
        Task<PasswordCheckResult> IsPassCorrectAsync(string userName, string passHash);

        /// <summary>
        /// Checks if a given <paramref name="userName" /> exists in the store.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns><c>true</c> if the user was found otherwise <c>false</c>.</returns>
        Task<bool> UserExistsAsync(string userName);

        #endregion
    }
}