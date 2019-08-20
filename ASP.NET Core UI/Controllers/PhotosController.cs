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

namespace ASP.NET_Core_UI.Controllers
{
    public class PhotosController : Controller
    {
        private readonly Services.Photo.PhotoService photoService;

        public PhotosController(Services.Photo.PhotoService photoService)
        {
            this.photoService = photoService;
        }

        

        public IActionResult Download(int? id)
        {
            if (id == null) return NotFound();
            var photo = photoService.GetPhoto(id.Value);

            if (photo == null) return NotFound();
            return File(photo.Binar,photo.MIMEType);

        }

        
    }
}
