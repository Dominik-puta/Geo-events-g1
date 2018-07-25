using System;

namespace GeoLocation.Model.Common
{
    public interface IEvent
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        float EntryFee { get; set; }
        int LimitedSpace { get; set; }
        string Organizer { get; set; }
        double Lat { get; set; }
        double Long { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        Guid EventCategoryId { get; set; }
        Guid EventSubcategoryId { get; set; }
        Guid VenueId { get; set; }
        Guid StatusId { get; set; }
    }
}
