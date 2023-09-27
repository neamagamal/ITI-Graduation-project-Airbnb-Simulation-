using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class Amenity
    {
        [Key]
        public int AmenityId { get; set; }
        public string AmenityName { get; set; }
        public string AmenityType { get; set; }

        public string AmenityImgSrc { get; set; }
        //NP
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
