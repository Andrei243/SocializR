﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public PostUserModel User { get; set; }
        public string Text { get; set; }
        public List<int> PhotoId { get; set; }
        public List<int> Reactions { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}
