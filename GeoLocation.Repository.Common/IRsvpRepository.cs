using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface IRsvpRepository
    {
        bool AddUser(Rsvp userInfo);
    }
}
