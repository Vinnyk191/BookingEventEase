using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingEventEase.Data;
using BookingEventEase.Models;

namespace BookingEventEase.Controllers
{
   //New Update
    
    public class BookingsController : Controller
    {
        private readonly BookingEventEaseDbContext _context;

        public BookingsController(BookingEventEaseDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue);
            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bookings booking)
        {
            if (ModelState.IsValid)
            {
                var isDoubleBooked = await _context.Bookings.AnyAsync(b =>
                    b.BookingDate == booking.BookingDate &&
                    b.VenueId == booking.VenueId);

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked for that date.");
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventID);
                    ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
                    return View(booking);
                }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventID);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventID);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Bookings booking)
        {
            if (id != booking.BookingID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var isDoubleBooked = await _context.Bookings.AnyAsync(b =>
                        b.BookingID != booking.BookingID &&
                        b.BookingDate == booking.BookingDate &&
                        b.VenueId == booking.VenueId);

                    if (isDoubleBooked)
                    {
                        ModelState.AddModelError("", "This venue is already booked for that date.");
                        return View(booking);
                    }

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.BookingID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventID);
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Search
        public IActionResult Search(int? eventTypeId, DateTime? startDate, DateTime? endDate, bool onlyAvailable = false)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.Venue)
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .AsQueryable();

            if (eventTypeId.HasValue && eventTypeId > 0)
                bookings = bookings.Where(b => b.Event.EventTypeId == eventTypeId);

            if (startDate.HasValue)
                bookings = bookings.Where(b => b.Event.EventDate >= startDate);

            if (endDate.HasValue)
                bookings = bookings.Where(b => b.Event.EventDate <= endDate);

            if (onlyAvailable)
                bookings = bookings.Where(b => b.Event.Venue.IsAvailable);

            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "EventTypeName");

            return View(bookings.ToList());
        }
    }
    

   
}
