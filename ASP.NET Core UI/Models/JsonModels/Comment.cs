using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.JsonModels
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int? ProfilePhoto { get; set; }
        public string Text { get; set; }

        public bool IsMine { get; set; }
    }
}
