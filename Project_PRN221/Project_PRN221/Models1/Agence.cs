using System;
using System.Collections.Generic;

namespace Project_PRN221.Models1
{
    public partial class Agence
    {
        public Agence()
        {
            Users = new HashSet<User>();
        }

        public int AgenceId { get; set; }
        public string AgenceName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
