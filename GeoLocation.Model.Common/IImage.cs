using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface IImage
    {
        Guid Id { get; set; }
        Guid EventId { get; set; }
        // image path, datatype?
        string FileName { get; set; }
        string Title { get; set; }
    }
}
