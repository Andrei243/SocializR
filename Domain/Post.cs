using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
            Photo = new HashSet<Photo>();
            Reaction = new HashSet<Reaction>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Vizibilitate { get; set; }
        public DateTime AddingMoment { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual ICollection<Reaction> Reaction { get; set; }
    }
}
