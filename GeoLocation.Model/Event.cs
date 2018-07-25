using GeoLocation.Model.Common;
using System;

namespace GeoLocation.Model
{
    class Event : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float EntryFee { get; set; }
        public int LimitedSpace { get; set; }
        public string Organizer { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EventCategoryId { get; set; }
        public Guid EventSubcategoryId { get; set; }
        public Guid VenueId { get; set; }
        public Guid StatusId { get; set; }

        public Event()
        {

        }
    }
}
