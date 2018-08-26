using GeoLocation.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository.Common
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetCommentsForEvent(Guid eventId);
        void AddComment(Comment newComment);
    }
}
