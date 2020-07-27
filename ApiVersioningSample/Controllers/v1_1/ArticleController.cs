namespace ApiVersioningSample.Controllers.v1_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Models.v1_1;

    [ApiVersion("1.1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ArticleController : ControllerBase
    {
        #region member vars

        private readonly ILogger<ArticleController> _logger;

        #endregion

        #region constructors and destructors

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves a list of all articles available.
        /// </summary>
        /// <returns>The unordered list of articles.</returns>
        /// <response code="200">At least 1 article was retrieved.</response>
        /// <response code="404">No orders found.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ArticleModel>), 200)]
        [ProducesResponseType(404)]
        public IEnumerable<ArticleModel> Get()
        {
            return new[]
            {
                new ArticleModel
                {
                    Id = 1,
                    Number = "ART-001",
                    Label = "Cool Stuff",
                    Price = 12
                }
            };
        }

        #endregion
    }
}