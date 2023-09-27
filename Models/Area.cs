using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class Area
    {
        [Key]
        public int AreaId { get; set; }
        [Required]
        public string AreaName { get; set; }
        public string AreaImg { get;set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        public double lat { get; set; }

        public double log { get; set; }
        //NP
        public City City { get; set; }

        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
