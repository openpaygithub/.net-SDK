using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace openpaySDKDemo.Models
{
    public class Modelopenpay
    {
        public class SignInSuccessModel
        {
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public class CheckOutModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "DOB")]
            public string DOB { get; set; }
            [Required]
            [Display(Name = "Subrub")]
            public string Subrub { get; set; }
            [Required]
            [Display(Name = "State")]
            public string State { get; set; }
            [Required]
            [Display(Name = "PostCode")]
            public string PostCode { get; set; }
        }
    }
}