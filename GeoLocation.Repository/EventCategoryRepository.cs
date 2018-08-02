using GeoLocation.Model;
using GeoLocation.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Microsoft.Extensions.Configuration;
using GeoLocation.Repository.Common;

namespace GeoLocation.Repository
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public EventCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public IEnumerable<EventCategory> GetEventCategories()
        {
            List<EventCategory> categories = new List<EventCategory>();
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"EventCategory\"";
                    command.Connection = conn;
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        EventCategory newCategory = new EventCategory()
                        {
                            Id = (Guid)dr["Id"],
                            Abrv = (string)dr["Abrv"],
                            Name = (dr["CategoryName"] is DBNull) ? string.Empty : (string)dr["CategoryName"],
                            Description = (dr["Description"] is DBNull) ? string.Empty : (string)dr["Description"],
                            DateCreated = (dr["DateCreated"] is DBNull) ? DateTime.Now : (DateTime)dr["DateCreated"]
                        };

                        categories.Add(newCategory);
                    }

                    return categories;
                }
            }
        }
    }
}
