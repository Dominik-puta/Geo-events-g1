using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface ICountry
    {
        Guid Id { get; set; }
        string Abrv { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
