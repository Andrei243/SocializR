﻿using System;
using System.Collections.Generic;
using Common;

namespace Domain 
{
    public partial class Reaction : IEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Users User { get; set; }
    }
}
