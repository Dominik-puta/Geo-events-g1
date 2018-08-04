using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocation.Web.Models
{
    public class AddViewModel
    {
        public Event NewEvent { get; set; }
        public IEnumerable<EventCategory> Categories { get; set; }
        public IEnumerable<EventSubCategory> SubCategories { get; set; }
        public IEnumerable<Venue> Venues { get; set; }
    }
}
