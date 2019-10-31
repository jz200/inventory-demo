using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Admin.Models
{
    public class RegisterUserPageModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage =
            "The password and confirmation password do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
