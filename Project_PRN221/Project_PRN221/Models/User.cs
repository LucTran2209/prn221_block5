using System;
using System.Collections.Generic;

namespace Project_PRN221.Models
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            SendDocuments = new HashSet<SendDocument>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Avatar { get; set; }
        public int RoleId { get; set; }
        public int AgenceId { get; set; }

        public virtual Agence Agence { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<SendDocument> SendDocuments { get; set; }
    }
}
