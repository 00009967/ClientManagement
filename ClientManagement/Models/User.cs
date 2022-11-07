using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientManagement.Models
{
    public class User 
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(60, ErrorMessage = "Length should not be more than 60 characters!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        [MaxLength(60, ErrorMessage = "Email should not me more than 60 charcters!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is required!")]
        [MaxLength(13, ErrorMessage = "Phone number should be in the format of +998912345678")]
        public string Phone { get; set; }
    }
}
