using GeoLocation.Model;
using GeoLocation.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using GeoLocation.Model.Common;
using Microsoft.Extensions.Configuration;

namespace GeoLocation.Repository
{
    public class EventRepository : IEventRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public EventRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public IEnumerable<IEvent> GetEvents()
        {
            List<Event> events = new List<Event>();
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM \"Event\"" +
                    "INNER JOIN \"EventCategory\" ON \"Event\".\"EventCategoryId\" = \"EventCategory\".\"Id\"" +
                    "INNER JOIN \"EventSubCategory\" ON \"Event\".\"EventSubCategoryId\" = \"EventSubCategory\".\"Id\"" +
                    "INNER JOIN \"Venue\" ON \"Event\".\"VenueId\" = \"Venue\".\"Id\"", conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Event newEvent = new Event()
                    {
                        Id = (Guid)dr["Id"],
                        Name = (string)dr["Name"],
                        Description = (string)dr["Description"],
                        EntryFee = (Decimal)dr["EntryFee"],
                        LimitedSpace = (int)dr["LimitedSpace"],
                        Organizer = (string)dr["Organizer"],
                        Lat = (double)dr["Lat"],
                        Long = (double)dr["Long"],
                        StartDate = (DateTime)dr["StartDate"],
                        EndDate = (DateTime)dr["EndDate"],
                        EventCategoryId = (dr["EventCategoryId"] is DBNull) ? Guid.Empty : (Guid)dr["EventCategoryId"],
                        EventSubCategoryId = (dr["EventSubCategoryId"] is DBNull) ? Guid.Empty : (Guid)dr["EventSubcategoryId"],
                        VenueId = (dr["VenueId"] is DBNull) ? Guid.Empty : (Guid)dr["VenueId"],
                        StatusId = (dr["StatusId"] is DBNull) ? Guid.Empty : (Guid)dr["StatusId"],
                        // joined columns
                        CategoryName = (string)dr["CategoryName"],
                        SubCategoryName = (string)dr["SubCategoryName"],
                        VenueName = (string)dr["VenueName"]
                    };
                    events.Add(newEvent);
                }
            }
            return events;
        }


        public void AddEvent(IEvent newEvent)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using(var command = new NpgsqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "INSERT INTO \"Event\" (\"Id\", \"Name\", \"Description\"" +
                        ", \"EntryFee\", \"LimitedSpace\", \"Organizer\", \"Lat\", \"Long\", \"StartDate\", \"EndDate\", \"EventCategoryId\", \"EventSubCategoryId\", \"VenueId\", \"StatusId\")" +
                        "VALUES (@id, @name, @desc" + 
                        ", @fee, @lspace, @org, @lat, @long, @start, @end, @catId, @subCatId, @venueId, @statusId)";
                    command.Parameters.AddWithValue("id", newEvent.Id);
                    command.Parameters.AddWithValue("name", newEvent.Name);
                    command.Parameters.AddWithValue("desc", newEvent.Description);
                    command.Parameters.AddWithValue("fee", newEvent.EntryFee);
                    command.Parameters.AddWithValue("lspace", newEvent.LimitedSpace);
                    command.Parameters.AddWithValue("org", newEvent.Organizer);
                    command.Parameters.AddWithValue("lat", newEvent.Lat);
                    command.Parameters.AddWithValue("long", newEvent.Long);
                    command.Parameters.AddWithValue("start", newEvent.StartDate);
                    command.Parameters.AddWithValue("end", newEvent.EndDate);
                    command.Parameters.AddWithValue("catId", newEvent.EventCategoryId);
                    command.Parameters.AddWithValue("subCatId", newEvent.EventSubCategoryId);
                    command.Parameters.AddWithValue("venueId", newEvent.VenueId);
                    command.Parameters.AddWithValue("statusId", newEvent.StatusId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Guid SearchForId(string item, string table, string column)
        {
            item = "'" + item + "'";
            table = "\"" + table + "\"";
            column = "\"" + column + "\"";

            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "SELECT \"Id\" FROM " + table +
                        " WHERE " + column + " = " + item;
                    var dr = command.ExecuteReader();
                    dr.Read();

                    Guid itemId = (Guid)dr["Id"];

                    return itemId;
                }
            }
        }
    }
}
