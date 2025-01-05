using System.ComponentModel.DataAnnotations;
using System;

namespace Web.Areas.Catalog.Models
{
    public class StaffUserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime BornDate { get; set; }
        public string Role { get; set; }
        public string PhoneNo { get; set; }

        public string FullName{ get; set; }
    }
}
