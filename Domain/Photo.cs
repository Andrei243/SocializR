using System;
using System.Collections.Generic;
using Common;

namespace Domain
{
    public partial class Photo : IEntity
    {
        public int Id { get; set; }
        public byte[] Binar { get; set; }
        public string MIMEType { get; set; }
        public int? AlbumId { get; set; }
        public int? PostId { get; set; }
        public int? Position { get; set; }

        public virtual Album Album { get; set; }
        public virtual Post Post { get; set; }
        public virtual Users Users { get; set; }
    }
}
