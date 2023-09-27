using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }
        [Required, MaxLength(200), MinLength(5)]
        public string PropertyTitle { get; set; }
        [MaxLength(450)]
        public string PropertyDescription { get; set; }
        [Required]
        public int PropertyCapacity { get; set; }
        [Required]
        public int PropertyBedsNum { get; set; }
        [Required]
        public int PropertyBedRooms { get; set; }
        [Required]
        public int PropertyBath { get; set; }
        [Required]
        public int PropertyPricePerNight { get; set; }
        [MaxLength(450)]
        public string PropertyHostInfo { get; set; }
        [ForeignKey("AppUser")]
        [Required]
        public string AppUserId { get; set; }
        [ForeignKey("Area")]
        [Required]
        public int AreaId { get; set; }
        [ForeignKey("Categoray")]
        [Required]
        public int CategorayId { get; set; }
        public bool Accepted { get; set; } = false;
        //NP
        public AppUser AppUser { get; set; }
        public Area Area { get; set; }
        public Categoray Categoray { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<PropertyImg> PropertyImgs { get; set; } = new HashSet<PropertyImg>();
        public ICollection<Amenity> Amenities { get; set; } = new HashSet<Amenity>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

    }
}
