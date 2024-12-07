using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class User_Registration_Model
    {
    
        public int UserId { get; set; }
        [Required(ErrorMessage ="Please Enter First Name")]
        [MaxLength(50,ErrorMessage ="Max Len is 50 charchter")]
        [MinLength(2, ErrorMessage = "Min Len is 2 charchter")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [MaxLength(50, ErrorMessage = "Max Len is 50 charchter")]
        [MinLength(2, ErrorMessage = "Min Len is 2 charchter")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [MaxLength(50, ErrorMessage = "Max Len is 50 charchter")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email_Id { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string User_Password { get; set; }
        public string Verify { get; set; }

        [Required(ErrorMessage = "Please Upload Image")]
        public string Profile_Image { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }

        
        public HttpPostedFileBase Image { get; set; }
    }
}