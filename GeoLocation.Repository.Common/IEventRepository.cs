using GeoLocation.Model;
using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocation.Repository.Common
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetEvents();
        void AddEvent(IEvent newEvent);
        void DeleteEvent(Guid EventId);
        Event GetEventById(Guid EventId);
        void UpdateEvent(Event updatedEvent);
        Status CheckStatus(Event newEvent);
    }
}
