using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = Policies.Identity.ReadOnly)]
        public IActionResult Get()
        {
            return Ok(new JsonResult(from c in User.Claims select new { c.Type, c.Value }));
        }
    }
}
