using GeoLocation.Model;
using GeoLocation.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace GeoLocation.Repository
{
    public class RsvpRepository : IRsvpRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public RsvpRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }
        public bool AddUser(Rsvp userInfo)
        {
            int limitedSpace;
            Int64 countedUsers;

            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT \"LimitedSpace\" FROM \"Event\" WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("id", userInfo.EventId);
                    command.Connection = conn;

                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();
                        limitedSpace = (int)dr["LimitedSpace"];
                    }
                }
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM \"Rsvp\" WHERE \"EventId\" = @id";
                    command.Parameters.AddWithValue("id", userInfo.EventId);
                    command.Connection = conn;
                    using (var dr = command.ExecuteReader())
                    {
                        dr.Read();
                        countedUsers = (Int64)dr["count"];
                    }
                }

                if (limitedSpace != 0 && countedUsers >= limitedSpace)
                {
                    return false;
                }
                else
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.CommandText = "INSERT INTO \"Rsvp\" (\"Id\", \"EventId\", \"FirstName\", \"LastName\", \"Email\") VALUES (@id, @eventId, @fname, @lname, @email)";
                        command.Parameters.AddWithValue("id", userInfo.Id);
                        command.Parameters.AddWithValue("eventId", userInfo.EventId);
                        command.Parameters.AddWithValue("fname", userInfo.FirstName);
                        command.Parameters.AddWithValue("lname", userInfo.LastName);
                        command.Parameters.AddWithValue("email", userInfo.Email);
                        command.Connection = conn;
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
        }
    }
}
