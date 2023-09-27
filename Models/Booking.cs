using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public int BookingCapacity { get; set; }
        //NP
        public Property Property { get; set; }
        public AppUser AppUser { get; set; }
    }
}
