using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly WebApiAppContext _context;

        public ArticleController(WebApiAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  ActionResult<IEnumerable<Article>> Get()
        {
            return _context.Articles;
        }
    }
}
