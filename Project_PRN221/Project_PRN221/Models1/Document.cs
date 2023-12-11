using System;
using System.Collections.Generic;

namespace Project_PRN221.Models1
{
    public partial class Document
    {
        public Document()
        {
            SendDocuments = new HashSet<SendDocument>();
        }

        public int DocumentId { get; set; }
        public string Title { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Content { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public bool? IsActive { get; set; }
        public int UserId { get; set; }
        public string HumanSign { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<SendDocument> SendDocuments { get; set; }
    }
}
