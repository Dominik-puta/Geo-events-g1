using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model
{
    public class Image : IImage
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Byte[] ImageFile { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
