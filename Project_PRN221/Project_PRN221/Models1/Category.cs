using System;
using System.Collections.Generic;

namespace Project_PRN221.Models1
{
    public partial class Category
    {
        public Category()
        {
            Documents = new HashSet<Document>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; }
    }
}
