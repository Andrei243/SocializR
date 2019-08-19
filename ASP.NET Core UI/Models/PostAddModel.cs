using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class PostAddModel
    {
        public string Text { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile Binar { get; set; }
        public string Visibility { get; set; }

    }
}
