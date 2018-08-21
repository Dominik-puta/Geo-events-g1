using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IRatingRepository
    {
        void AddRating(int value, Guid eventId);
        double GetAvgRating(Guid eventId);
    }
}
