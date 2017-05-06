namespace codingfreaks.AspNetIdentity.Services.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Logic.Shared.Interfaces;
    using Logic.Shared.TransportModels;

    /// <summary>
    /// Provides access to user related actions.
    /// </summary>
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        #region member vars

        private readonly IUserRepository _userRepository;

        #endregion

        #region constructors and destructors

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves a list of all currently available users in the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var result = await _userRepository.GetUsersAsync();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not retrieve users.");
            }
        }

        /// <summary>
        /// Tries to retrieve a single user searching by it's unique <paramref name="userName" />.
        /// </summary>
        /// <param name="userName">The unique user name to search for.</param>
        /// <returns>The user information or <c>null</c> if no user was found.</returns>
        [Route("GetByUserName")]
        public async Task<HttpResponseMessage> Get(string userName)
        {
            try
            {
                var result = await _userRepository.GetUserByUserNameAsync(userName);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not retrieve users.");
            }
        }

        /// <summary>
        /// Tries to retrieve a single user searching by it's unique <paramref name="id" />.
        /// </summary>
        /// <param name="id">The unique database id of the user.</param>
        /// <returns>The user information or <c>null</c> if no user was found.</returns>
        public async Task<HttpResponseMessage> Get(long id)
        {
            try
            {
                var result = await _userRepository.GetUserByIdAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not retrieve user.");
            }
        }

        /// <summary>
        /// Retrieves a list of all role names a user with a given <paramref name="id" /> is member of.
        /// </summary>
        /// <param name="id">The database id of the user.</param>
        /// <returns>The list of role names.</returns>
        [Route("{id}/RoleNames")]
        public async Task<HttpResponseMessage> GetRoleNames(long id)
        {
            try
            {
                var result = await _userRepository.GetRoleNamesAsync(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not retrieve users.");
            }
        }

        /// <summary>
        /// Adds a single user using the <paramref name="model" /> data.
        /// </summary>
        /// <param name="model">The data of the user.</param>
        /// <returns>The database id of the user or <c>null</c> if the user could not be created.</returns>
        public async Task<HttpResponseMessage> Post(UserTransportModel model)
        {
            try
            {
                var result = await _userRepository.AddUserAsnyc(model, "User");
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create user.");
            }
        }

        #endregion
    }
}