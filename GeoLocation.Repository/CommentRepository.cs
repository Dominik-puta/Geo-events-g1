using GeoLocation.Model;
using GeoLocation.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public CommentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public IEnumerable<Comment> GetCommentsForEvent(Guid eventId)
        {
            List<Comment> comments = new List<Comment>();
            using(conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using(var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"Comment\" WHERE \"EventId\" = @eventId ORDER BY \"DateCreated\" DESC";
                    command.Parameters.AddWithValue("eventId", eventId);
                    command.Connection = conn;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Comment newComment = new Comment
                        {
                            Id = (Guid)dr["Id"],
                            EventId = (Guid)dr["EventId"],
                            Text = (string)dr["Text"],
                            FirstName = (string)dr["FirstName"],
                            LastName = (string)dr["LastName"],
                            DateCreated = (DateTime)dr["DateCreated"]
                        };
                        comments.Add(newComment);
                    }

                    return comments;
                }
            }
        }

        public void AddComment(Comment newComment)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "INSERT INTO \"Comment\" (\"Id\", \"EventId\", \"Text\", \"FirstName\", \"LastName\", \"DateCreated\")" +
                        "VALUES (@id, @eventId, @text, @fname, @lname, @dateCreated)";
                    command.Parameters.AddWithValue("id", newComment.Id);
                    command.Parameters.AddWithValue("eventId", newComment.EventId);
                    command.Parameters.AddWithValue("text", newComment.Text);
                    command.Parameters.AddWithValue("fname", newComment.FirstName);
                    command.Parameters.AddWithValue("lname", newComment.LastName);
                    command.Parameters.AddWithValue("dateCreated", newComment.DateCreated);
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
