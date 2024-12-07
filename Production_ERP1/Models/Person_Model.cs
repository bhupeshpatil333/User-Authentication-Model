using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Production_ERP1.Models
{
    public class Person_Model
    {
        public int Person_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Person Name")]
        public string Person_Name { get; set; }

        [Required(ErrorMessage = "Please Enter Person Contact Number")]
        [MaxLength(10, ErrorMessage = "Enter 10 Digit Number")]
        [MinLength(10, ErrorMessage = "Enter 10 Digit Number")]
        public string Person_Contact { get; set; }
        public Nullable<int> PersonType_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [MaxLength(50, ErrorMessage = "Max Len is 50 charchter")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email_Id { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}