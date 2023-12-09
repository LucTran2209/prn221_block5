using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN221.Models
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            SendDocumentUserIdReceiveNavigations = new HashSet<SendDocument>();
            SendDocumentUserIdSendNavigations = new HashSet<SendDocument>();
        }

        public int UserId { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Không được trống")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Không được trống")]
        public string Password { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Không được trống")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Không được trống")]
        [RegularExpression(@"^(\+)?[0-9]{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Không được trống")]
        public int AgenceId { get; set; }

        public virtual Agence? Agence { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<SendDocument> SendDocumentUserIdReceiveNavigations { get; set; }
        public virtual ICollection<SendDocument> SendDocumentUserIdSendNavigations { get; set; }
    }
}
