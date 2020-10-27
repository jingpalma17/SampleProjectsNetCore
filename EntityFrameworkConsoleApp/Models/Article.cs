using System;
namespace EntityFrameworkConsoleApp.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
