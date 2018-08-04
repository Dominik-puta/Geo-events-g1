using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocation.Web.Models
{
    public class EventDetailsViewModel
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public EventCategory EventCategory { get; set; }
        public EventSubCategory EventSubCategory { get; set; }
        public Venue Venue { get; set; }
    }
}
