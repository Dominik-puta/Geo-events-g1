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
        private ICommentRepository _commentRepository;
        private IStatusRepository _statusRepository;

        public HomeController(
            IEventRepository eventRepository, 
            IEventCategoryRepository eventCategoryRepository,
            IEventSubCategoryRepository eventSubCategoryRepository,
            IVenueRepository venueRepository,
            IRsvpRepository rsvpRepository,
            ICommentRepository commentRepository,
            IStatusRepository statusRepository
        )
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventSubCategoryRepository = eventSubCategoryRepository;
            _venueRepository = venueRepository;
            _rsvpRepository = rsvpRepository;
            _commentRepository = commentRepository;
            _statusRepository = statusRepository;
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
            model.NewEvent.StatusId = Guid.Parse("7ca65c86-0e39-465f-874d-fcb3c9183f1b"); // privremeno rjesenje, stavlja status na upcoming
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
            eventDetails.Comments = _commentRepository.GetCommentsForEvent(eventDetails.EventId);
            eventDetails.NewComment = new Comment { EventId = eventDetails.EventId };
            return View(eventDetails);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Rsvp(Rsvp userInfo)
        {
            userInfo.Id = Guid.NewGuid();
            bool succesful = _rsvpRepository.AddUser(userInfo);
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = userInfo.EventId, UserLimitReached = !succesful });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddComment(Comment newComment)
        {
            newComment.Id = Guid.NewGuid();
            newComment.DateCreated = DateTime.Now;
            _commentRepository.AddComment(newComment);
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = newComment.EventId });
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
