using GeoLocation.Model;
using GeoLocation.Repository.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using GeoLocation.Model.Common;

namespace GeoLocation.Repository
{
    public class DbRepository : IRepository
    {
        private NpgsqlConnection conn = new NpgsqlConnection("Server=192.168.0.230;User Id=g20184;" + "Password=kQT976qeo;Database=2018-g4-geo-events-grupa-1;");

        public IEnumerable<IEvent> GetEvents()
        {
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM \"Event\"", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            List<Event> events = new List<Event>();
            while (dr.Read())
            {
                Event newEvent = new Event()
                {
                    Id = (Guid)dr["Id"],
                    Name = (string)dr["Name"],
                    Description = (string)dr["Description"],
                    //EntryFee = dr.GetDecimal(dr.GetOrdinal("EntryFee")),
                    LimitedSpace = (int)dr["LimitedSpace"],
                    Organizer = (string)dr["Organizer"],
                    Lat = (double)dr["Lat"],
                    Long = (double)dr["Long"],
                    StartDate = (DateTime)dr["StartDate"],
                    EndDate = (DateTime)dr["EndDate"],
                    //EventCategoryId = (Guid)dr["EventCategoryId"],
                    //EventSubcategoryId = (Guid)dr["EventSubcategoryId"],
                    //VenueId = (Guid)dr["VenueId"],
                    //StatusId = (Guid)dr["StatusId"]
                };
                events.Add(newEvent);
            }
            conn.Close();
            return (events);
        }
    }
}
