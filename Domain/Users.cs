using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Users
    {
        public Users()
        {
            Album = new HashSet<Album>();
            Comment = new HashSet<Comment>();
            FriendshipIdReceiverNavigation = new HashSet<Friendship>();
            FriendshipIdSenderNavigation = new HashSet<Friendship>();
            InterestsUsers = new HashSet<InterestsUsers>();
            Post = new HashSet<Post>();
            Reaction = new HashSet<Reaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public DateTime BirthDay { get; set; }
        public int? LocalityId { get; set; }
        public string SexualIdentity { get; set; }
        public string Vizibility { get; set; }

        public virtual Locality Locality { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Friendship> FriendshipIdReceiverNavigation { get; set; }
        public virtual ICollection<Friendship> FriendshipIdSenderNavigation { get; set; }
        public virtual ICollection<InterestsUsers> InterestsUsers { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Reaction> Reaction { get; set; }
    }
}
