using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Purchase_Line_Model
    {
        public int Purchase_Line_Id { get; set; }
        public Nullable<int> Purchase_Header_Id { get; set; }

        public Nullable<int> Product_Id { get; set; }
        public Nullable<int> Item_Id { get; set; }

        [Required(ErrorMessage = "Please Select Tax here")]
        public Nullable<int> Tax_Id { get; set; }
        public Nullable<decimal> Quantity { get; set; }

        [Required(ErrorMessage = "Please Enter Rate")]
        public Nullable<decimal> Rate { get; set; }
        public Nullable<decimal> Tax_Amount { get; set; }

        [Required(ErrorMessage = "Please Enter Percentage %")]
        public Nullable<decimal> Discount_Percentage { get; set; }
        public Nullable<decimal> Basic_Amount { get; set; }
        public Nullable<decimal> Tax_Total_Amount { get; set; }
        public Nullable<decimal> Discount_Amount { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
    }
}