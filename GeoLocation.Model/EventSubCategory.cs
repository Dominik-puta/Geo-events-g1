using System;
using System.Collections.Generic;
using System.Text;
using GeoLocation.Model.Common;

namespace GeoLocation.Model
{
    public class EventSubCategory : IEventSubCategory
    {
        public Guid Id { get; set; }
        public string Abrv { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
