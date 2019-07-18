using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Interest
    {
        public Interest()
        {
            InterestsUsers = new HashSet<InterestsUsers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<InterestsUsers> InterestsUsers { get; set; }
    }
}
