﻿using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Friendship : IEntity
    {
        public int IdSender { get; set; }
        public int IdReceiver { get; set; }
        public bool? Accepted { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Users IdReceiverNavigation { get; set; }
        public virtual Users IdSenderNavigation { get; set; }
    }
}
