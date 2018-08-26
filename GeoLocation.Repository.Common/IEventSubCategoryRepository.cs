using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IEventSubCategoryRepository
    {
        IEnumerable<EventSubCategory> GetSubCategories();
        EventSubCategory GetSubCategoryById(Guid subCategoryId);
    }
}
