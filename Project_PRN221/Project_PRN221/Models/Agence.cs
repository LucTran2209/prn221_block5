using System;
using System.Collections.Generic;

namespace Project_PRN221.Models
{
    public partial class Agence
    {
        public Agence()
        {
            Documents = new HashSet<Document>();
        }

        public int AgenceId { get; set; }
        public string AgenceName { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; }
    }
}
