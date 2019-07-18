using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime AddingMoment { get; set; }

        public virtual Post Post { get; set; }
        public virtual Users User { get; set; }
    }
}
