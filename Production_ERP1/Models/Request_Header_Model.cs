using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Request_Header_Model
    {
        public int Request_Header_Id { get; set; }

        [Required(ErrorMessage = "Please Select Code")]
        public string Request_Header_Code { get; set; }

        [Required(ErrorMessage = "Please Choose Date here!")]
        public Nullable<System.DateTime> Request_Header_Date { get; set; }
    }
}