using System.ComponentModel.DataAnnotations;

namespace BookingEventEase.Models
{
    public class Venue
    {
        [Key]
        
        public int VenueId { get; set; }
        [Required]
        public string VenueName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string ImageUrl { get; set; } = "https://via.placeholder.com/150";
        public bool IsAvailable { get; set; }
        public ICollection<Event> Events { get; set; } //Added
    }
}
