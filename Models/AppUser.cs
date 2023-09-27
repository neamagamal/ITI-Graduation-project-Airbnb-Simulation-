using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class AppUser : IdentityUser
    {
        [Required ,MaxLength(10)]
        public string FirstName { get; set; }

        [Required, MaxLength(10)]
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }




        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "You must be 18 years or older to register")]
        public int userAge { get; set; }
        //[Required(ErrorMessage = "Phone number is required")]
        //[RegularExpression(@"^01(0|1|2|5)\d{8}$", ErrorMessage = "Phone number must be a valid Egypt phone number")]
        //public int Phone { get; set; }

        //NP
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();


    }
}
