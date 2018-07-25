using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Model.Common
{
    public interface IComment
    {
        Guid Id { get; set; }
        Guid EventId { get; set; }
        string Text { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateCreated { get; set; }
    }
}
