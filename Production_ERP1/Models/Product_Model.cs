using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Product_Model
    {
        public int Product_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Product Name")]
        public string Product_Name { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}