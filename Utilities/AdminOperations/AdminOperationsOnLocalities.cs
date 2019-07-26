using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using DataAccess;

namespace Utilities
{
    class AdminOperationsOnLocalities
    {
        private Authenticator user;

        public AdminOperationsOnLocalities(Users user)
        {
            this.user = new Authenticator(user);
        }

        public bool DeleteLocality(Locality locality)
        {
            if (user.isAdmin())
            {
                using (var ctx = new SocializRContext())
                {
                    ctx.Locality.Remove(locality);
                    ctx.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool EditLocality(Locality pre, Locality post)
        {
            if (user.isAdmin())
            {
                using (var ctx = new SocializRContext())
                {
                    ctx.Locality.Update(pre);
                    if (pre.Name != post.Name) pre.Name = post.Name;
                    if (pre.CountyId != post.CountyId) { pre.CountyId = post.CountyId; pre.County = post.County; }
                    ctx.Locality.Update(pre);
                    ctx.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public bool AddLocality(Locality newLocality)
        {
            if (user.isAdmin())
            {
                using(var ctx = new SocializRContext())
                {
                    ctx.Locality.Add(newLocality);
                    ctx.SaveChanges();
                    return true;

                }

            }
            return false;
        }



    }
}
