using GeoLocation.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoLocation.Web.Models
{
    public class UpdateViewModel
    {
        public Event Event { get; set; } 
        public IFormFile UploadedImage { get; set; }
    }
}
