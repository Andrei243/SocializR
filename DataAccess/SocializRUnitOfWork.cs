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
        }

        public IRepository<Post> Posts
        {
            get
            {
                return Posts ?? (Posts = new BaseRepository<Post>(DbContext));
            }
            private set
            {
                Posts = value;
            }

        }
        public IRepository<Users> Users
        {
            get
            {
                return Users ?? (Users = new BaseRepository<Users>(DbContext));
            }
            private set
            {
                Users = value;
            }

        }

        public IRepository<Album> Albums
        {
            get
            {
                return Albums ?? (Albums = new BaseRepository<Album>(DbContext));
            }
            private set
            {
                Albums = value;
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                return Comments ?? (Comments = new BaseRepository<Comment>(DbContext));
            }
            private set
            {
                Comments = value;
            }

        }

        public IRepository<County> Counties
        {
            get
            {
                return Counties ?? (Counties = new BaseRepository<County>(DbContext));
            }
            private set
            {
                Counties = value;
            }
        }

        public IRepository<Friendship> Friendships
        {
            get
            {
                return Friendships ?? (Friendships = new BaseRepository<Friendship>(DbContext));
            }
            private set
            {
                Friendships = value;
            }
        }

        public IRepository<Interest> Interests
        {
            get
            {
                return Interests ?? (Interests = new BaseRepository<Interest>(DbContext));
            }
            private set
            {
                Interests = value;
            }
        }

        public IRepository<InterestsUsers> InterestsUserss
        {
            get
            {
                return InterestsUserss ?? (InterestsUserss = new BaseRepository<InterestsUsers>(DbContext));
            }
            private set
            {
                InterestsUserss = value;
            }
        }
        public IRepository<Locality> Localities
        {
            get
            {
                return Localities ?? (Localities = new BaseRepository<Locality>(DbContext));
            }
            private set
            {
                Localities = value;
            }
        }

        public IRepository<Photo> Photos
        {
            get
            {
                return Photos ?? (Photos = new BaseRepository<Photo>(DbContext));
            }
            private set
            {
                Photos = value;
            }
        }

        public IRepository<Reaction> Reactions
        {
            get
            {
                return Reactions ?? (Reactions = new BaseRepository<Reaction>(DbContext));
            }
            private set
            {
                Reactions = value;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                return Roles ?? (Roles = new BaseRepository<Role>(DbContext));
            }
            private set
            {
                Roles = value;
            }
        }

    }
}
