using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Services.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Logic.Shared.Interfaces;
    using Logic.Shared.TransportModels;

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