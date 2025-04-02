using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingEventEase.Models
{
    public class Event
    {
        [Key]

        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; }

        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        [Required]
        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public virtual Venue Venue { get; set; }
    }
}
