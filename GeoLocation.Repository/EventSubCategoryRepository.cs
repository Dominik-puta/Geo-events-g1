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
    public class EventSubCategoryRepository : IEventSubCategoryRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public EventSubCategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public IEnumerable<EventSubCategory> GetSubCategories()
        {
            List<EventSubCategory> subCategories = new List<EventSubCategory>();
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"EventSubCategory\"";
                    command.Connection = conn;
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        EventSubCategory newSubCategory = new EventSubCategory()
                        {
                            Id = (Guid)dr["Id"],
                            Abrv = (string)dr["Abrv"],
                            Name = (string)dr["SubCategoryName"],
                            DateCreated = (DateTime)dr["DateCreated"]
                        };

                        subCategories.Add(newSubCategory);
                    }

                    return subCategories;
                }
            }
        }
    }
}