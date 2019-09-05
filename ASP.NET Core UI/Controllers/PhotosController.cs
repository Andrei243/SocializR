using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain;
using ASP.NET_Core_UI.Models;
using System.IO;
using ASP.NET_Core_UI.Models.ProfileModels;
using ASP.NET_Core_UI.Code.Base;
using AutoMapper;

namespace ASP.NET_Core_UI.Controllers
{
    public class PhotosController : BaseController
    {
        private readonly Services.Photo.PhotoService photoService;

        public PhotosController(Services.Photo.PhotoService photoService,IMapper imapper):
            base(imapper)
        {
            this.photoService = photoService;
        }

        
        [HttpGet]
        public IActionResult Download(int? id)
        {
            if (id == null) return NotFound();
            if (!photoService.CanSeePhoto(id.Value)) return ForbidView();
            var photo = photoService.GetPhoto(id.Value);

            if (photo == null) return NotFound();

            return File(photo.Binary,photo.MIMEType);

        }

        
    }
}
