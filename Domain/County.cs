using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class County
    {
        public County()
        {
            Locality = new HashSet<Locality>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Locality> Locality { get; set; }
    }
}
