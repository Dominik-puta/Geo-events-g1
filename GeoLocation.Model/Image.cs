using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model
{
    class Image : IImage
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        // image path, datatype?
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
