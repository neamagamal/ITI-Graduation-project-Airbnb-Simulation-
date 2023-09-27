using System.ComponentModel.DataAnnotations.Schema;

namespace AirBnB.Models
{
    public class PropertyImg
    {
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public string ImgSrc { get; set; }

        //NP
        public Property property { get; set; }
    }
}
