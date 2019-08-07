using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABIY_One.Models.Data_Models
{
    public class Customer
    {
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string FirstName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Fist Name must be atleast 3 characters long", MinimumLength = 3)]
        public string LastName { get; set; }
        [Key]
        [Required]
        [Display(Name = "Email")]
        [DataType(dataType: DataType.EmailAddress)]
        //[RegularExpression(pattern: @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(dataType: DataType.PhoneNumber)]
        [RegularExpression(pattern: @"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(maximumLength: 10, ErrorMessage = "SA Contact Number must be exactly 10 digits long", MinimumLength = 10)]
        public string phone { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        [Display(Name = "Physical Address")]
        public string address { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

    }
}