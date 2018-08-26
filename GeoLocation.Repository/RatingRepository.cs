using GeoLocation.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoLocation.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public RatingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public void AddRating(int value, Guid eventId)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "INSERT INTO \"Rating\" (\"Id\", \"EventId\", \"Value\") VALUES (@id, @eventId, @value)";
                    command.Parameters.AddWithValue("id", Guid.NewGuid());
                    command.Parameters.AddWithValue("eventId", eventId);
                    command.Parameters.AddWithValue("value", value);
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                }
            }
        }

        public double GetAvgRating(Guid eventId)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                var ratingValues = new List<int>();

                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT \"Value\" FROM \"Rating\" WHERE \"EventId\" = @eventId";
                    command.Parameters.AddWithValue("eventId", eventId);
                    command.Connection = conn;
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int value = (int)dr["Value"];
                        ratingValues.Add(value);
                    }
                }

                double average = ratingValues.Count > 0 ? ratingValues.Average() : 0.0;
                return average;
            }
        }
    }
}
