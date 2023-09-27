using System.ComponentModel.DataAnnotations;

namespace AirBnB.ViewModels
{
    public class RoleFormViewModel
    {
        [StringLength(256),Required]
        public string Name { get; set; }
    }
}
