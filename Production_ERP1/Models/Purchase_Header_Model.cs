using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Purchase_Header_Model
    {
        public int Purchase_Header_Id { get; set; }

        [Required(ErrorMessage = "Please Select Code")]
        public Nullable<int> Requirement_Header_Id { get; set; }

        [Required(ErrorMessage = "Please Select Person Name")]
        public Nullable<int> Person_Id { get; set; }

        [Required(ErrorMessage = "Please Select Date")]
        public Nullable<System.DateTime> PO_Date { get; set; }
        public Nullable<decimal> Basic_Amount { get; set; }
        public Nullable<decimal> Tax_Amount { get; set; }
        public Nullable<decimal> Discount_Amount { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
    }
}