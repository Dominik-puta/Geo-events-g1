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
using GeoLocation.Repository.Common;

namespace GeoLocation.Web.Controllers
{
    public class HomeController : Controller
    {
        private IEventRepository _eventRepository;
        private IEventCategoryRepository _eventCategoryRepository;
        private IEventSubCategoryRepository _eventSubCategoryRepository;
        private IVenueRepository _venueRepository;

        public HomeController(
            IEventRepository eventRepository, 
            IEventCategoryRepository eventCategoryRepository,
            IEventSubCategoryRepository eventSubCategoryRepository,
            IVenueRepository venueRepository
            )
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventSubCategoryRepository = eventSubCategoryRepository;
            _venueRepository = venueRepository;
        }

        public IActionResult Index()
        {
            var events = _eventRepository.GetEvents();
            return View(events);
        }

        public IActionResult Add()
        {
            ViewData["Message"] = "";

            try
            {
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
                        EventCategoryId = Guid.Parse(Request.Form["EventCategory"]),
                        EventSubCategoryId = Guid.Parse(Request.Form["EventSubCategory"]),
                        VenueId = Guid.Parse(Request.Form["Venue"])
                    };

                    _eventRepository.AddEvent(newEvent);

                    ViewData["Message"] = "Added";
                }
            }
            catch (Exception)
            {
                ViewData["Message"] = "Input is invalid.";
            }

            AddViewModel addViewModel = new AddViewModel()
            {
                Categories = _eventCategoryRepository.GetEventCategories(),
                SubCategories = _eventSubCategoryRepository.GetSubCategories(),
                Venues = _venueRepository.GetVenues()
            };

            return View(addViewModel);
        }

        public IActionResult DeleteEvent(Guid id)
        {
            _eventRepository.DeleteEvent(id);
            return RedirectToAction("Index");
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
