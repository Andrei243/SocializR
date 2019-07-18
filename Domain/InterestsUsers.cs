using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class InterestsUsers
    {
        public int InterestId { get; set; }
        public int UserId { get; set; }

        public virtual Interest Interest { get; set; }
        public virtual Users User { get; set; }
    }
}
