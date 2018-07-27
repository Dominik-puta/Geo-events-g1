using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface IImage
    {
        Guid Id { get; set; }
        Guid EventId { get; set; }
        Byte[] ImageFile { get; set; }
        string FileName { get; set; }
        string Title { get; set; }
    }
}
