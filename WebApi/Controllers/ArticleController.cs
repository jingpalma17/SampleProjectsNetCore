using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Article> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Article
            {
                Title = "sadasdasd"
            })
            .ToArray();
        }
    }
}
