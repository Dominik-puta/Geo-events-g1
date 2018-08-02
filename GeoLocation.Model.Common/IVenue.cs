using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface IVenue
    {
        Guid Id { get; set; }
        string Description { get; set; }
        string Address { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Name { get; set; }
    }
}
