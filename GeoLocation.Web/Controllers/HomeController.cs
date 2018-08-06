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
        private IRsvpRepository _rsvpRepository;

        public HomeController(
            IEventRepository eventRepository, 
            IEventCategoryRepository eventCategoryRepository,
            IEventSubCategoryRepository eventSubCategoryRepository,
            IVenueRepository venueRepository,
            IRsvpRepository rsvpRepository
            )
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventSubCategoryRepository = eventSubCategoryRepository;
            _venueRepository = venueRepository;
            _rsvpRepository = rsvpRepository;
        }

        public IActionResult Index()
        {
            var events = _eventRepository.GetEvents();
            return View(events);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddViewModel model)
        {
            model.NewEvent.Id = Guid.NewGuid();
            _eventRepository.AddEvent(model.NewEvent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        { 
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

        public IActionResult EventDetails(EventDetailsViewModel eventDetails)
        {
            eventDetails.Event = _eventRepository.GetEventById(eventDetails.EventId);
            eventDetails.EventCategory = _eventCategoryRepository.GetCategoryById(eventDetails.Event.EventCategoryId);
            eventDetails.EventSubCategory = _eventSubCategoryRepository.GetSubCategoryById(eventDetails.Event.EventSubCategoryId);
            eventDetails.Venue = _venueRepository.GetVenueById(eventDetails.Event.VenueId);
            eventDetails.Rsvp = new Rsvp { EventId = eventDetails.EventId };
            return View(eventDetails);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Rsvp(Rsvp userInfo)
        {
            userInfo.Id = Guid.NewGuid();
            bool succesful = _rsvpRepository.AddUser(userInfo);
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = userInfo.EventId, UserLimitReached = !succesful });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
