using GeoLocation.Model;
using GeoLocation.Repository.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLocation.Repository
{
    public class ImageRepository : IImageRepository
    {
        private NpgsqlConnection conn = null;
        private IConfiguration _configuration;
        private string _conStr = string.Empty;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MainConnection");
        }

        public void AddImage(Image image)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "INSERT INTO \"Image\" (\"Id\", \"EventId\", \"Image\", \"FileName\", \"Title\") VALUES (@id, @eventId, @image, @filename, @title)";
                    command.Parameters.AddWithValue("id", image.Id);
                    command.Parameters.AddWithValue("eventId", image.EventId);
                    command.Parameters.AddWithValue("image", image.ImageFile);
                    command.Parameters.AddWithValue("filename", image.FileName);
                    command.Parameters.AddWithValue("title", image.Title);
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                }
            }
        }

        public Image GetImage(Guid eventId)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT * FROM \"Image\" WHERE \"EventId\" = @eventId";
                    command.Parameters.AddWithValue("eventId", eventId);
                    command.Connection = conn;
                    var dr = command.ExecuteReader();

                    dr.Read();
                    Image newImage = new Image
                    {
                        Id = (Guid)dr["Id"],
                        EventId = (Guid)dr["EventId"],
                        ImageFile = (byte[])dr["Image"],
                        FileName = (string)dr["FileName"],
                        Title = (string)dr["Title"]
                    };

                    return newImage;
                }
            }
        }

        public void DeleteImage(Guid eventId)
        {
            using (conn = new NpgsqlConnection(_conStr))
            {
                conn.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "DELETE FROM \"Image\" WHERE \"EventId\" = @eventId";
                    command.Parameters.AddWithValue("eventId", eventId);
                    command.Connection = conn;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
