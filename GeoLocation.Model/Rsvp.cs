using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model
{
    public class Rsvp : IRsvp
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
