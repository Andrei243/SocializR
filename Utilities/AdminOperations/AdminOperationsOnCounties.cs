using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Utilities.AdminOperations
{
    class AdminOperationsOnCounties
    {
        private Authenticator user;

        public AdminOperationsOnCounties(Users user)
        {
            this.user = new Authenticator(user);
        }


        public bool DeleteCounty(County county)
        {
            if (county.Locality.Count==0) {
                using(var ctx = new SocializRContext())
                {
                    county = ctx.County.Include(c => c.Locality).FirstOrDefault(c => c.Id == county.Id);
                }
            }

            if (county.Locality.Count != 0) return false;
            if (user.isAdmin())
            {
                using(var ctx = new SocializRContext())
                {
                    ctx.County.Remove(county);
                    ctx.SaveChanges();
                    ctx.SaveChanges();

                }

            }
            return false;
        }

        public bool EditCounty(County pre,County post)
        {
            if (user.isAdmin())
            {
                using(var ctx = new SocializRContext())
                {
                    ctx.County.Update(pre);
                    if (pre.Name != post.Name) pre.Name = post.Name;
                    ctx.SaveChanges();
                    return true;
                    

                }


            }
            return false;
        }

        public bool AddCounty(County newCounty)
        {
            if (user.isAdmin())
            {
                using(var ctx = new SocializRContext())
                {
                    ctx.County.Add(newCounty);
                    ctx.SaveChanges();
                    return true;
                }

            }
            return false;

        }
    }
}
