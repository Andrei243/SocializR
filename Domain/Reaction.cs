using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Reaction
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Users User { get; set; }
    }
}
