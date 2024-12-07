using Production_ERP1.Db_Context;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class Purchase_HeaderController : Controller
    {
        // GET: Purchase_Header
        public ActionResult GetTaxPer(int tax_id)
        {
            decimal taxper;
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var getTax = (from x in db.Taxes.Where(x => x.Tax_Id == tax_id) select new { x.Tax_Percentage }).FirstOrDefault();
                decimal.TryParse(getTax.Tax_Percentage.ToString(), out taxper);
                var res = new { taxpers = taxper };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            Purchase_Header_Model header_obj = new Purchase_Header_Model();
            var line = new List<Purchase_Line_Model>();
            var sp_header_line = new List<sp_joinHeader_Result>();

            ViewBag.POCodeDDL = POCodeDDL();
            ViewBag.PersonDDL = PersonDDL();
            Purchase_Header_Line_Model model = new Purchase_Header_Line_Model()
            {
                header_obj = header_obj,
                line_obj = line,
                Header_Line_sp = sp_header_line
            };

            return View(model);
        }

        public ActionResult Purchase_HeaderView()
        {
            ViewBag.POCodeDDL = POCodeDDL();
            ViewBag.PersonDDL = PersonDDL();
            return View();
        }
        public ActionResult Purchase_LineView(int headr_id)
        {
            var sp_line_header = new List<sp_joinHeader_Result>();
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                sp_line_header = db.sp_joinHeader(headr_id).ToList();
            }

            return PartialView("Purchase_LineView", sp_line_header);
        }
        public List<SelectListItem> POCodeDDL()
        {

            var POcode = new List<SelectListItem>();

            POcode.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.PO_Request_Header
                            select new
                            {
                                x.Request_Header_Id,
                                x.Request_Header_Code
                            }).ToList();
                foreach (var item in data)
                {
                    POcode.Add(new SelectListItem { Value = item.Request_Header_Id.ToString(), Text = item.Request_Header_Code.ToString() });
                }
            }
            return POcode;
        }
        public List<SelectListItem> TaxDDL()
        {

            var POcode = new List<SelectListItem>();

            POcode.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.Taxes
                            select new
                            {
                                x.Tax_Id,
                                x.Tax_Name
                            }).ToList();
                foreach (var item in data)
                {
                    POcode.Add(new SelectListItem { Value = item.Tax_Id.ToString(), Text = item.Tax_Name.ToString() });
                }
            }
            return POcode;
        }
        public ActionResult GetPoData(int hedar_id)
        {
            var sp_result = new List<sp_joinHeader_Result>();

            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                sp_result = db.sp_joinHeader(hedar_id).ToList();
            }
            ViewBag.tax = TaxDDL();
            return PartialView("Purchase_LineView", sp_result);
        }
        public List<SelectListItem> PersonDDL()
        {
            var personlist = new List<SelectListItem>();

            personlist.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.People
                            select new
                            {
                                x.Person_Id,
                                x.Person_Name
                            }).ToList();
                foreach (var item in data)
                {
                    personlist.Add(new SelectListItem { Value = item.Person_Id.ToString(), Text = item.Person_Name.ToString() });
                }
            }
            return personlist;
            ;
        }

        public ActionResult SaveOrUpdate(FormCollection collection)
        {

            // Initialization
            int header_pk;
            DateTime purchaseDate = Convert.ToDateTime(collection.Get("PO_Date"));
            //DateTime purchaseDate = Convert.ToDateTime(collection.Get("PO_Date"));
            int requirementCode = Convert.ToInt32(collection.Get("Requirement_Header_Id"));
            int personId = Convert.ToInt32(collection.Get("Person_Id"));

            // Initialize totals
            decimal totalBasicAmount = 0m;
            decimal totalDiscountAmount = 0m;
            decimal totalTaxAmount = 0m;
            decimal totalAmount = 0m;

            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Create and save Purchase_Header
                        Purchase_Header header = new Purchase_Header
                        {
                            PO_Date = purchaseDate,
                            Requirement_Header_Id = requirementCode,
                            Person_Id = personId
                        };

                        db.Entry(header).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        header_pk = header.Purchase_Header_Id;

                        // Retrieve line item data
                        string[] ProductIds = collection.Get("item.Product_Id").Split(',');
                        string[] ItemIds = collection.Get("item.Item_Id").Split(',');
                        string[] TaxIds = collection.Get("item.tax_id")?.Split(',') ?? new string[0]; // handle null
                        string[] Quantities = collection.Get("item.Qty").Split(',');
                        string[] Rates = collection.Get("item.rate").Split(',');
                        string[] BasicAmounts = collection.Get("item.Basic_Amount")?.Split(',') ?? new string[0];
                        string[] DiscountPercents = collection.Get("item.discoutper")?.Split(',') ?? new string[0];
                        string[] DiscountAmounts = collection.Get("item.descoutamt")?.Split(',') ?? new string[0];
                        string[] TaxAmounts = collection.Get("item.taxamount")?.Split(',') ?? new string[0];
                        string[] TotalAmounts = collection.Get("item.toalamout")?.Split(',') ?? new string[0];

                        // Save Purchase_Line entries and calculate totals
                        for (int i = 0; i < ProductIds.Length; i++)
                        {
                            int productId = Convert.ToInt32(ProductIds[i]);
                            int itemId = Convert.ToInt32(ItemIds[i]);
                            int taxId = (int)(TaxIds.Length > i && int.TryParse(TaxIds[i], out var tempTaxId) ? tempTaxId : (int?)null);
                            decimal quantity = Convert.ToDecimal(Quantities[i]);
                            decimal rate = Convert.ToDecimal(Rates[i]);
                            decimal basicAmountLine = BasicAmounts.Length > i && decimal.TryParse(BasicAmounts[i], out var tempBasicLine) ? tempBasicLine : 0m;
                            decimal discountPercent = DiscountPercents.Length > i && decimal.TryParse(DiscountPercents[i], out var tempDiscountLine) ? tempDiscountLine : 0m;
                            decimal discountAmountLine = DiscountAmounts.Length > i && decimal.TryParse(DiscountAmounts[i], out var tempDiscountAmtLine) ? tempDiscountAmtLine : 0m;
                            decimal taxAmountLine = TaxAmounts.Length > i && decimal.TryParse(TaxAmounts[i], out var tempTaxLine) ? tempTaxLine : 0m;
                            decimal totalAmountLine = TotalAmounts.Length > i && decimal.TryParse(TotalAmounts[i], out var tempTotalLine) ? tempTotalLine : 0m;

                            // Update totals
                            totalBasicAmount += basicAmountLine;
                            totalDiscountAmount += discountAmountLine;
                            totalTaxAmount += taxAmountLine;
                            totalAmount += totalAmountLine;

                            // Create Purchase_Line_Table entry
                            Purchase_Line_Table line = new Purchase_Line_Table
                            {
                                Purchase_Header_Id = header_pk,
                                Product_Id = productId,
                                Item_Id = itemId,
                                Tax_Id = taxId, // Set the tax_id here
                                Quantity = quantity,
                                Rate = rate,
                                Basic_Amount = basicAmountLine,
                                Discount_Percentage = discountPercent,
                                Discount_Amount = discountAmountLine,
                                Tax_Amount = taxAmountLine,
                                Total_Amount = totalAmountLine
                            };

                            db.Entry(line).State = System.Data.Entity.EntityState.Added;
                        }

                        // Save all line items
                        db.SaveChanges();

                        // Update header totals
                        header.Basic_Amount = totalBasicAmount;
                        header.Discount_Amount = totalDiscountAmount;
                        header.Tax_Amount = totalTaxAmount;
                        header.Total_Amount = totalAmount;

                        // Update header
                        db.Entry(header).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        
                    }
                }
            }

            return RedirectToAction("Index");
        }

    }
        
    
    
}