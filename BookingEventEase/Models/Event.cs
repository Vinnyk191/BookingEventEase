using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingEventEase.Models
{
    public class Event
    {
        [Key]
        [Required]
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        [Required]
        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        
        public int EventTypeId { get; set; }
        public Venue Venue { get; set; }
        public EventType EventType { get; set; }    


        public ICollection<Bookings> Bookings { get; set; }  // Added
    }
}
