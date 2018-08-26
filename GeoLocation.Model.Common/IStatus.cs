using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface IStatus
    {
        Guid Id { get; set; }
        string Abrv { get; set; }
        string Name { get; set; }
    }
}
