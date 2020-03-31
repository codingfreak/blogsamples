namespace ApiVersioningSample.Controllers.v1_0
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Models.v1_0;

    [ApiVersion("1.0")]
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
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IEnumerable<ArticleModel> Get()
        {
            return new[]
            {
                new ArticleModel
                {
                    Id = 1,
                    Number = "ART-001",
                    Label = "Cool Stuff"
                }
            };
        }

        #endregion
    }
}