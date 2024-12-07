using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Tax_Model
    {
        public int Tax_Id { get; set; }

        [Required(ErrorMessage = "Please Enter Tax Name")]
        public string Tax_Name { get; set; }

        [Required(ErrorMessage = "Required Tax Percentage")]
        public Nullable<decimal> Tax_Percentage { get; set; }
    }
}