using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoLocation.Web.Models;
using GeoLocation.Model;
using GeoLocation.Model.Common;
using GeoLocation.Repository;
using Npgsql;
using GeoLocation.Repository.Common;

namespace GeoLocation.Web.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var events = _repository.GetEvents();
            return View(events);
        }

        public IActionResult Add()
        {
            ViewData["Message"] = "";

            if (Request.HasFormContentType)
            {
                Event newEvent = new Event()
                {
                    Id = Guid.NewGuid(),
                    Name = Request.Form["Name"],
                    Description = Request.Form["Description"],
                    EntryFee = Decimal.Parse(Request.Form["EntryFee"]),
                    LimitedSpace = int.Parse(Request.Form["LimitedSpace"]),
                    Organizer = Request.Form["Organizer"],
                    Lat = double.Parse(Request.Form["Lat"]),
                    Long = double.Parse(Request.Form["Long"]),
                    StartDate = DateTime.Parse(Request.Form["StartDate"]),
                    EndDate = DateTime.Parse(Request.Form["EndDate"]),
                    CategoryName = Request.Form["CategoryName"],
                    SubCategoryName = Request.Form["SubCategoryName"],
                    VenueName = Request.Form["VenueName"],
                };
                
                newEvent.EventCategoryId = _repository.SearchForId(newEvent.CategoryName, "EventCategory", "CategoryName");
                newEvent.EventSubcategoryId = _repository.SearchForId(newEvent.SubCategoryName, "EventSubCategory", "SubCategoryName");
                newEvent.VenueId = _repository.SearchForId(newEvent.VenueName, "Venue", "VenueName");

                _repository.AddEvent(newEvent);

                ViewData["Message"] = "Added";
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
