using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi
{
    public class WebApiAppContext : DbContext
    {
        public WebApiAppContext(DbContextOptions<WebApiAppContext> options)
            : base(options) { }

        public DbSet<Article> Articles { get; set; }
       
    }
}
