using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class FeedModel
    {
        public List<PostModel> Posts { get; set; }
        public PostAddModel PostAdd { get; set; }
        public int CurrentPage { get; set; }

    }
}
