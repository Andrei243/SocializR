using System;
using System.Collections.Generic;

namespace Domain
{
    public partial class Friendship
    {
        public int IdSender { get; set; }
        public int IdReceiver { get; set; }
        public bool Accepted { get; set; }

        public virtual Users IdReceiverNavigation { get; set; }
        public virtual Users IdSenderNavigation { get; set; }
    }
}
