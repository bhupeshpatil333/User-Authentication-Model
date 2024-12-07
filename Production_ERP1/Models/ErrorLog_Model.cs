using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public partial class ErrorLog_Model
    {
        public int ErrorId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorController { get; set; }
        public string ErrorFunction { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorData { get; set; }
    }
}