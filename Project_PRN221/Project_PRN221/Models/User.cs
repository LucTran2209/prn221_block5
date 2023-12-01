using System;
using System.Collections.Generic;

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
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<SendDocument> SendDocumentUserIdReceiveNavigations { get; set; }
        public virtual ICollection<SendDocument> SendDocumentUserIdSendNavigations { get; set; }
    }
}
