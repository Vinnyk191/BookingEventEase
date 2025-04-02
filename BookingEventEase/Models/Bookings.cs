using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingEventEase.Models
{
    public class Bookings
    {
         [Key]
        public int BookingID { get; set; }
        
        [ForeignKey("Event")]
        public int EventID { get; set; }
        public Event Event { get; set; }
        [Required]
        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        
        public DateTime BookingDate { get; set; }
    }
}
