using Production_ERP1.Db_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Production_ERP1.Models
{
    public class Purchase_Header_Line_Model
    {
        public Purchase_Header_Model header_obj { get; set; }

        public IEnumerable<Purchase_Line_Model> line_obj { get; set; }

        public IEnumerable <sp_joinHeader_Result> Header_Line_sp {  get; set; }
    }
}