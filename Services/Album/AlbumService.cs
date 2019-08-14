﻿using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services.Album
{
    public class AlbumService : Base.BaseService
    {
        public readonly CurrentUser CurrentUser;

        public AlbumService(CurrentUser currentUser,SocializRUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.CurrentUser = currentUser;
        }

        public List<Domain.Album> getAll(int idUser)
        {
            return unitOfWork.Albums.Query.Where(e => e.UserId == idUser).AsNoTracking().Include(e=>e.Photo).AsNoTracking().OrderBy(e => e.Id).ToList();
        }

        

    }
}
