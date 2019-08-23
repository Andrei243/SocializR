﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models.FeedModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public PostUserModel User { get; set; }
        public string Text { get; set; }
    }
}
