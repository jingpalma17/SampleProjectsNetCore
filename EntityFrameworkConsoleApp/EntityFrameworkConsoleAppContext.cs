using System;
using EntityFrameworkConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsoleApp
{
    public class EntityFrameworkConsoleAppContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1,1433;Initial Catalog=SampleProjectsNetCore;User ID=sa;Password=Password123!;MultipleActiveResultSets=true");
        }
    }
}
