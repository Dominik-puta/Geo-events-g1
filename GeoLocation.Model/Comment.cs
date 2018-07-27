using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model
{
    public class Comment : IComment
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Text { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
