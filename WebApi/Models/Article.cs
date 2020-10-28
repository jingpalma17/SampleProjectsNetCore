using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public string Category { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
