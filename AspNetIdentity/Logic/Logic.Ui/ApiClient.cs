namespace codingfreaks.AspNetIdentity.Logic.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Models;

    using RestSharp;

    using Shared.TransportModels;

    /// <summary>
    /// Wrapper for conveniant REST requests against the API.
    /// </summary>
    public static class ApiClient
    {
        #region methods

        /// <summary>
        /// Calls the POST method on /User to create a new user in the database.
        /// </summary>
        /// <param name="user">The user informations to use.</param>
        /// <returns>The database id of the newly created user on success or <c>null</c> if any error occured.</returns>
        public static async Task<long?> CreateUserAsync(ApplicationUser user)
        {
            var data = Mapper.Map<ApplicationUser, UserTransportModel>(user);
            return await SendDataAsync<UserTransportModel, long?>("User", data);
        }

        /// <summary>
        /// Calls the DELETE method on /User to delete an existing user.
        /// </summary>
        /// <param name="user">The user informations (only the id is needed).</param>
        /// <returns><c>true</c> if the operation succeeded otherwise <c>false</c>.</returns>
        public static async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            return await RetrieveResultAsync<bool>($"User/{user.Id}", Method.DELETE);
        }

        /// <summary>
        /// Calls the GET method on /User/{id}/RoleNames to retrieve a list of all member roles.
        /// </summary>
        /// <param name="id">The database id of the user.</param>
        /// <returns>The list of role names the user is member of.</returns>
        public static async Task<List<string>> GetUserRoleNamesAsync(long id)
        {
            return await RetrieveResultAsync<List<string>>($"User/{id}/RoleNames");
        }

        /// <summary>
        /// Calls the GET method on /User to retrieve a list of all users currently available in the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        public static async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            var result = await RetrieveResultAsync<List<UserTransportModel>>("User");
            if (!result?.Any() ?? true)
            {
                return Enumerable.Empty<ApplicationUser>();
            }
            return result.Select(Mapper.Map<UserTransportModel, ApplicationUser>);
        }

        /// <summary>
        /// Calls the PATCH method in /User to update the user data.
        /// </summary>
        /// <param name="user">The updated user informations.</param>
        /// <returns><c>true</c> if the operation succeeded otherwise <c>false</c>.</returns>
        public static async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var data = Mapper.Map<ApplicationUser, UserTransportModel>(user);
            return await SendDataAsync<UserTransportModel, bool>("User", data, Method.PATCH);
        }

        /// <summary>
        /// Sends a request using a URL and retrieves the result.
        /// </summary>
        /// <remarks>This is usually used to perform GET requests.</remarks>
        /// <typeparam name="TResult">The type of the expected result from the API.</typeparam>
        /// <param name="relativeUrl">The trailing URL part without the informations passed to the <see cref="Client" />.</param>
        /// <param name="method">Optional definition of HTTP method to use (defaults to GET).</param>
        /// <param name="parameters">Optional list of additional URL parameters.</param>
        /// <returns>The result of the API or the default of <typeparamref name="TResult" /> if an error occurs.</returns>
        private static async Task<TResult> RetrieveResultAsync<TResult>(string relativeUrl = null, Method method = Method.GET, IEnumerable<Parameter> parameters = null)
        {
            var request = new RestRequest(relativeUrl, method);
            if (parameters?.Any() ?? false)
            {
                request.Parameters.AddRange(parameters);
            }
            try
            {
                var response = await Client.ExecuteTaskAsync<TResult>(request).ConfigureAwait(false);
                return response.Data;
            }
            catch
            {
                return default(TResult);
            }
        }

        /// <summary>
        /// Sends a request to the API that contains some data in the body.
        /// </summary>
        /// <remarks>This is usually used to perform POST requests.</remarks>
        /// <typeparam name="TData">The type of the data model passed to the API.</typeparam>
        /// <typeparam name="TResult">The type of the expected result from the API.</typeparam>
        /// <param name="relativeUrl">The trailing URL part without the informations passed to the <see cref="Client" />.</param>
        /// <param name="data">The data to send.</param>
        /// <param name="method">Optional definition of HTTP method to use (defaults to POST).</param>
        /// <param name="parameters">Optional list of additional URL parameters.</param>
        /// <returns>The result of the API or the default of <typeparamref name="TResult" /> if an error occurs.</returns>
        private static async Task<TResult> SendDataAsync<TData, TResult>(string relativeUrl, TData data, Method method = Method.POST, IEnumerable<Parameter> parameters = null)
        {
            var request = new RestRequest(relativeUrl, method)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(data);
            if (parameters?.Any() ?? false)
            {
                request.Parameters.AddRange(parameters);
            }
            try
            {
                var response = await Client.ExecuteTaskAsync<TResult>(request).ConfigureAwait(false);
                return response.Data;
            }
            catch
            {
                return default(TResult);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The RESTSharp client instance configured with the API base url.
        /// </summary>
        private static RestClient Client => new RestClient(ConfigurationManager.AppSettings["Services.BaseUrl"]);

        #endregion
    }
}