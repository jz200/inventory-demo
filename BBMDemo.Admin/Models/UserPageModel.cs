using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Admin.Models
{
    public class UserPageModel
    {
        [Required, Display(Name ="User Id")]
        public string Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, Display(Name ="Is Admin")]
        public bool IsAdmin { get; set; }
        
        
    }
}
