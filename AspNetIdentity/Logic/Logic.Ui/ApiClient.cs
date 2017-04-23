using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Ui
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    using AutoMapper;

    using Models;

    using RestSharp;

    using Shared.TransportModels;

    public static class ApiClient
    {
        #region methods

        public static async Task<long?> CreateUserAsync(ApplicationUser user)
        {
            var data = Mapper.Map<ApplicationUser, UserTransportModel>(user);
            return await SendDataAsync<UserTransportModel, long?>("User", data);
        }

        public static async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            return await RetrieveResultAsync<bool>($"User/{user.Id}", Method.DELETE);
        }

        public static async Task<List<string>> GetUserRoleNamesAsync(long id)
        {
            return await RetrieveResultAsync<List<string>>($"User/{id}/RoleNames");
        }

        public static async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            var result = await RetrieveResultAsync<List<UserTransportModel>>("User");
            if (!result?.Any() ?? true)
            {
                return Enumerable.Empty<ApplicationUser>();
            }            
            return result.Select(Mapper.Map<UserTransportModel, ApplicationUser>);
        }

        public static async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var data = Mapper.Map<ApplicationUser, UserTransportModel>(user);
            return await SendDataAsync<UserTransportModel, bool>("User", data, Method.PATCH);
        }

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

        private static RestClient Client => new RestClient(ConfigurationManager.AppSettings["Services.BaseUrl"]);

        #endregion
    }
}