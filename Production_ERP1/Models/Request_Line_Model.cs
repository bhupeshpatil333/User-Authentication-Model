using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Request_Line_Model
    {
        public int Request_Line_Id { get; set; }
        public Nullable<int> Request_Header_Id { get; set; }

        [Required(ErrorMessage = "Please Select Product Name")]
        public Nullable<int> Product_Id { get; set; }

        [Required(ErrorMessage = "Please Select Item Name")]
        public Nullable<int> Item_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Quantity")]
        public Nullable<decimal> Qty { get; set; }
    }
}