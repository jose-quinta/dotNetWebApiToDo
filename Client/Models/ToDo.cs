using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        [Display(Name = "Done")]
        public bool Done { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated")]
        public DateTime UpdatedAt { get; set; }
    }
}