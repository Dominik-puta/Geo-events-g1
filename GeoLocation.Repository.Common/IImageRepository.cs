using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IImageRepository
    {
        void AddImage(Image image);
        Image GetImage(Guid eventId);
        void DeleteImage(Guid EventId);
    }
}
