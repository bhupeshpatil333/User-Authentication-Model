//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Production_ERP1.Db_Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Purchase_Header
    {
        public int Purchase_Header_Id { get; set; }
        public Nullable<int> Requirement_Header_Id { get; set; }
        public Nullable<int> Person_Id { get; set; }
        public Nullable<System.DateTime> PO_Date { get; set; }
        public Nullable<decimal> Basic_Amount { get; set; }
        public Nullable<decimal> Tax_Amount { get; set; }
        public Nullable<decimal> Discount_Amount { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
    }
}