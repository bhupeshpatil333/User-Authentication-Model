using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Item_Model
    {
        public int Item_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Item Name")]
        public string Item_Name { get; set; }
        public Nullable<int> Product_Id { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}