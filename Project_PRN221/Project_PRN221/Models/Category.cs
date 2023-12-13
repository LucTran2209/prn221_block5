using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN221.Models
{
    public partial class Category
    {
        public Category()
        {
            Documents = new HashSet<Document>();
        }
        [BindProperty]
        [Required(ErrorMessage = "Không được để trống")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; }
    }
}
