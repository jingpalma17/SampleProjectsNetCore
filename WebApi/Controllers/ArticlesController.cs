using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly WebApiAppContext _context;

        public ArticlesController(WebApiAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Policy = Policies.Article.ReadOnly)]
        public  ActionResult<IEnumerable<Article>> GetArticles()
        {
            return _context.Articles;
        }
 
        [HttpGet("{id}")]
        [Authorize(Policy = Policies.Article.ReadOnly)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [Authorize(Policy = Policies.Article.Write)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Article>> CreateArticle(Article article)
        {
            article = new Article
            {
                Title = article.Title,
                Description = article.Description,
                Category = article.Category,
                Deleted = article.Deleted
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetArticle),
                new { id = article.Id },
                article);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policies.Article.Write)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            var fountArticle = await _context.Articles.FindAsync(id);
            if (fountArticle == null)
            {
                return NotFound();
            }

            fountArticle.Title = article.Title;
            fountArticle.Description = article.Description;
            fountArticle.Category = article.Category;
            fountArticle.Deleted = article.Deleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
