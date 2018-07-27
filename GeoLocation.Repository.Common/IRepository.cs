using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IRepository
    {
        IEnumerable<IEvent> GetEvents();
    }
}
