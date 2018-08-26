using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model
{
    public class Status : IStatus
    {
        public Guid Id { get; set; }
        public string Abrv { get; set; }
        public string Name { get; set; }
    }
}
