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
using System.IO;
using Geolocation;
using Npgsql;

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
        private IImageRepository _imageRepository;
        private IRatingRepository _ratingRepository;

        public HomeController(
            IEventRepository eventRepository, 
            IEventCategoryRepository eventCategoryRepository,
            IEventSubCategoryRepository eventSubCategoryRepository,
            IVenueRepository venueRepository,
            IRsvpRepository rsvpRepository,
            ICommentRepository commentRepository,
            IStatusRepository statusRepository,
            IImageRepository imageRepository,
            IRatingRepository ratingRepository
        )
        {
            _eventRepository = eventRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventSubCategoryRepository = eventSubCategoryRepository;
            _venueRepository = venueRepository;
            _rsvpRepository = rsvpRepository;
            _commentRepository = commentRepository;
            _statusRepository = statusRepository;
            _imageRepository = imageRepository;
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index(string searchString, double lat, double lng, float radius)
        {

            IEnumerable<Event> events = new List<Event>();

            try
            {
                events = _eventRepository.GetEvents().Result;
            }
            catch (TimeoutException)
            {
                return RedirectToAction("Error");
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Name.ToLower().Contains(searchString.ToLower()) || e.Description.ToLower().Contains(searchString.ToLower()));
            }

            foreach (var item in events)
            {
                item.Image = _imageRepository.GetImage(item.Id);
                item.EventStatus = _eventRepository.CheckStatus(item);
            }

            if(!Double.IsNaN(lat) && !Double.IsNaN(lng) && lat != 0 && lng != 0)
            {
                events = events.Where(e => GeoCalculator.GetDistance(e.Lat, e.Long, lat, lng, 1, DistanceUnit.Kilometers) <= radius);                
            }

            return View(events.ToList());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddViewModel model)
        {
            Image newImage = new Image
            {
                Id = Guid.NewGuid(),
                FileName = model.Image.FileName,
                Title = model.Image.Name
            };

            if(model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newImage.ImageFile = fileBytes;
                }
            }

            model.NewEvent.Id = Guid.NewGuid();
            model.NewEvent.StatusId = Guid.Parse("7ca65c86-0e39-465f-874d-fcb3c9183f1b"); // privremeno rjesenje, stavlja status na upcoming
            newImage.EventId = model.NewEvent.Id;
            _eventRepository.AddEvent(model.NewEvent);
            _imageRepository.AddImage(newImage);
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

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            AddViewModel model = new AddViewModel
            {
                NewEvent = _eventRepository.GetEventById(id),
                Categories = _eventCategoryRepository.GetEventCategories(),
                SubCategories = _eventSubCategoryRepository.GetSubCategories(),
                Venues = _venueRepository.GetVenues()
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(AddViewModel model)
        {
            if (!(model.Image is null))
            {
                Image newImage = new Image
                {
                    Id = Guid.NewGuid(),
                    FileName = model.Image.FileName,
                    Title = model.Image.Name
                };

                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    newImage.ImageFile = fileBytes;
                }

                _imageRepository.DeleteImage(model.NewEvent.Id);
                newImage.EventId = model.NewEvent.Id;
                _imageRepository.AddImage(newImage);
            }

            model.NewEvent.StatusId = Guid.Parse("7ca65c86-0e39-465f-874d-fcb3c9183f1b"); // privremeno rjesenje, stavlja status na upcoming
            _eventRepository.UpdateEvent(model.NewEvent);
            return RedirectToAction("Index");
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
            eventDetails.Image = _imageRepository.GetImage(eventDetails.EventId);
            eventDetails.Rsvp = new Rsvp { EventId = eventDetails.EventId };
            eventDetails.Comments = _commentRepository.GetCommentsForEvent(eventDetails.EventId);
            eventDetails.NewComment = new Comment { EventId = eventDetails.EventId };
            eventDetails.RatingAverage = _ratingRepository.GetAvgRating(eventDetails.EventId);
            if (eventDetails.UserLimitReached) ViewData["Message"] = "Sva mjesta za ovaj događaj su popunjena.";
            if (eventDetails.DuplicateUser) ViewData["Message"] = "Već ste se prijavili za ovaj događaj.";
            return View(eventDetails);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Rsvp(Rsvp userInfo)
        {
            userInfo.Id = Guid.NewGuid();
            bool succesful = true;
            bool duplicateUser = false;
            try
            {
                succesful = _rsvpRepository.AddUser(userInfo);
            }
            catch (NpgsqlException ex)
            {
                if (ex.ErrorCode == -2147467259) duplicateUser = true;
            }
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = userInfo.EventId, UserLimitReached = !succesful, DuplicateUser = duplicateUser });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddComment(Comment newComment)
        {
            newComment.Id = Guid.NewGuid();
            newComment.DateCreated = DateTime.Now;
            _commentRepository.AddComment(newComment);
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = newComment.EventId });
        }

        public IActionResult Rate(int value, Guid eventId)
        {
            _ratingRepository.AddRating(value, eventId);
            return RedirectToAction("EventDetails", new EventDetailsViewModel { EventId = eventId });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
