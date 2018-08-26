using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IStatusRepository
    {
        Status GetStatusByAbrv(string abrv);
    }
}
