    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Person_Type_Model
    {
        public int PersonType_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Person Name")]
        public string Person_Name { get; set; }
    }
}