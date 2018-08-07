using GeoLocation.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GeoLocation.Model
{
    public class Event : IEvent
    {
        public Event() { }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal EntryFee { get; set; }
        public int LimitedSpace { get; set; }
        public string Organizer { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EventCategoryId { get; set; }
        public Guid EventSubCategoryId { get; set; }
        public Guid VenueId { get; set; }
        public Guid StatusId { get; set; }

        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string VenueName { get; set; }
        public string StatusName { get; set; }
    }
}
