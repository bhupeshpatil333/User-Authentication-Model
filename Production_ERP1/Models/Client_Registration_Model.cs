using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Client_Registration_Model
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Please Enter Client Name")]
        [MaxLength(50, ErrorMessage = "Max Len is 50 charchter")]
        [MinLength(2, ErrorMessage = "Min Len is 2 charchter")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Please Enter Client Business Name")]
        public string ClientBusiness_Name { get; set; }

        [Required(ErrorMessage = "Please Upload Logo")]
        public string Client_Logo { get; set; }

        [Required(ErrorMessage = "Please Enter Client GST Number")]
        public string Client_GSTNo { get; set; }

        [Required(ErrorMessage = "Please Upload GST Document")]
        public string Client_GST_Document { get; set; }
        public Nullable<int> UserId { get; set; }

        [Required(ErrorMessage = "Please Enter Client Contact Number")]
        [MaxLength(10, ErrorMessage = "Enter 10 Digit Number")]
        [MinLength(10, ErrorMessage = "Enter 10 Digit Number")]
        public string Client_Contact { get; set; }

        public HttpPostedFileBase Logo { get; set; }
        public HttpPostedFileBase Documennt { get; set; }
    }
}
