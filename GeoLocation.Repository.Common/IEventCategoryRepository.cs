using GeoLocation.Model;
using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IEventCategoryRepository
    {
        IEnumerable<EventCategory> GetEventCategories();
        EventCategory GetCategoryById(Guid categoryId);
    }
}
