using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirBnB.Models
{
    public class Review
    {
        [Key] 
        public int ReviewId { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        public int ReviewRate { get; set; }
        public string ReviewComment { get; set; }
        //NP
        public Property Property { get; set; }
        public AppUser AppUser { get; set; }

    }
}
