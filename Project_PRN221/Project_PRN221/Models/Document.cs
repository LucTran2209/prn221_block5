using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN221.Models
{
    public partial class Document
    {
        public Document()
        {
            SendDocuments = new HashSet<SendDocument>();
        }

        public int DocumentId { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Không được trống")]
        public string DocumentNumber { get; set; } = null!;
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        public string Content { get; set; } = null!;
        [Required(ErrorMessage = "Không được trống")]
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
