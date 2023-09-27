using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace AirBnB.Models
{
    public class Categoray
    {
        [Key]
        public int CategorayId { get; set; }
        [Required]
        public string CategorayName { get; set; }
        //NP
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();

        public string Categoryphotosrc { get; set; }

    }
}
