using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Domain;
using DataAccess.Base;

namespace DataAccess
{
    public class SocializRUnitOfWork : Base.BaseUnitOfWork
    {
        public SocializRUnitOfWork(SocializRContext context) : base(context)
        {
            Posts = new BaseRepository<Post>(DbContext);
            Users = new BaseRepository<Users>(DbContext);
            Albums = new BaseRepository<Album>(DbContext);
            Comments = new BaseRepository<Comment>(DbContext);
            Counties = new BaseRepository<County>(DbContext);
            Friendships = new BaseRepository<Friendship>(DbContext);
            Interests = new BaseRepository<Interest>(DbContext);
            InterestsUserss = new BaseRepository<InterestsUsers>(DbContext);
            Localities = new BaseRepository<Locality>(DbContext);
            Photos = new BaseRepository<Photo>(DbContext);
            Reactions = new BaseRepository<Reaction>(DbContext);
            Roles = new BaseRepository<Role>(DbContext);
        }

        public IRepository<Post> Posts{ get; }
        public IRepository<Users> Users{ get; }

        public IRepository<Album> Albums { get; }

        public IRepository<Comment> Comments { get; }

        public IRepository<County> Counties { get; }

        public IRepository<Friendship> Friendships { get; }

        public IRepository<Interest> Interests { get; }

        public IRepository<InterestsUsers> InterestsUserss { get; }
        public IRepository<Locality> Localities { get; }

        public IRepository<Photo> Photos { get; }

        public IRepository<Reaction> Reactions { get; }

        public IRepository<Role> Roles { get; }

    }
}
