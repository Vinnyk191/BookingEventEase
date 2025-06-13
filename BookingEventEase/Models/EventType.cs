using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace BookingEventEase.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }
        [Required]
        public string EventTypeName { get; set; } 
    }
}
