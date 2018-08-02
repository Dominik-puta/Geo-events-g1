using GeoLocation.Model;
using GeoLocation.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository
{
    public class VenueRepository : IVenueRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public VenueRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public IEnumerable<Venue> GetVenues()
        {

            List<Venue> venues = new List<Venue>();
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"Venue\"";
                    command.Connection = conn;
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        Venue newVenue = new Venue()
                        {
                            Id = (Guid)dr["Id"],
                            Name = (string)dr["VenueName"],
                            Description = (dr["Description"] is DBNull) ? string.Empty : (string)dr["Description"],
                            Address = (dr["Address"] is DBNull) ? string.Empty : (string)dr["Address"],
                            Phone = (dr["Phone"] is DBNull) ? string.Empty : (string)dr["Phone"],
                            Email = (dr["Email"] is DBNull) ? string.Empty : (string)dr["Email"]
                        };
                        venues.Add(newVenue);
                    }
                    return venues;
                }
            }
        }
    }
}
