using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Request_Header_LIne_Model
    {
        public Request_Header_Model header_obj {  get; set; }

        public IEnumerable<Request_Line_Model> line_obj{ get; set; }
    }
}