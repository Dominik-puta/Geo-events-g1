using GeoLocation.Model;
using GeoLocation.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public StatusRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public Status GetStatusByAbrv(string abrv)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"Status\" WHERE \"Abrv\" = @abrv";
                    command.Parameters.AddWithValue("abrv", abrv);
                    command.Connection = conn;
                    var dr = command.ExecuteReader();
                    dr.Read();

                    Status newStatus = new Status
                    {
                        Id = (Guid)dr["Id"],
                        Abrv = (string)dr["Abrv"],
                        Name = (string)dr["StatusName"]
                    };

                    return newStatus;
                }
            }
        }
    }
}
