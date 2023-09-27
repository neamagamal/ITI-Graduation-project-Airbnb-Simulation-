using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        public string CityName { get; set; }
        public string CityImg { get; set; }
        //NP
        public ICollection<Area> Areas { get; set; } = new HashSet<Area>();
    }
}
