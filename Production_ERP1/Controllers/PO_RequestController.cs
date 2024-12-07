using Production_ERP1.Db_Context;
using Production_ERP1.ErrorManagement;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class PO_RequestController : Controller
    {
        int UserId;

        public bool IsValid()
        {
            if (Session["UserId"] != null && Session["FirstName"] != null && Session["LastName"] != null && Session["EmailId"] != null)
            {
                UserId = Convert.ToInt32(Session["UserId"]);
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: PO_Request
        public ActionResult Index()
        {
            if (IsValid() == true)
            {
                try
                {
                    Request_Header_Model header = new Request_Header_Model();
                    header.Request_Header_Code = GenerateCode();
                    var line = new List<Request_Line_Model>();
                    ViewBag.ProductDDL = ProductDDL();
                    ViewBag.ItemDDL = ItemDDL();
                    Request_Header_LIne_Model model = new Request_Header_LIne_Model()
                    {
                        header_obj = header,
                        line_obj = line,
                    };

                    return View(model);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    // Log the error using your existing error handling function
                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "PO_Request", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }


            
        }
        public List<SelectListItem> ProductDDL()
        {

            var productlist = new List<SelectListItem>();

            productlist.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.Products
                            select new
                            {
                                x.Product_Id,
                                x.Product_Name
                            }).ToList();
                foreach (var item in data)
                {
                    productlist.Add(new SelectListItem { Value = item.Product_Id.ToString(), Text = item.Product_Name.ToString() });
                }

            }
            return productlist;
        }
        public List<SelectListItem> ItemDDL()
        {
            var Itemlist = new List<SelectListItem>();

            Itemlist.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.Items
                            select new
                            {
                                x.Item_Id,
                                x.Item_Name
                            }).ToList();
                foreach (var item in data)
                {
                    Itemlist.Add(new SelectListItem { Value = item.Item_Id.ToString(), Text = item.Item_Name.ToString() });
                }


            }

            return Itemlist;
;
        }
        public ActionResult RequestHeader()
        {
           if(IsValid() == true)
            {
                try
                {
                    Request_Header_Model model = new Request_Header_Model();
                    model.Request_Header_Code = GenerateCode();
                    return View(model);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    // Log the error using your existing error handling function
                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "PO_Request", "RequestHeader", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
           else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public string GenerateCode()
        {

            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                //var max = (from x in db.PO_Request_Header select new { x.Request_Header_Id }).Max();
                //int maxcount = Convert.ToInt32(max);
                //maxcount = maxcount + 1;

                var max = db.PO_Request_Header.Count();
                max++;

                string code;
                if (max.ToString().Length == 1)
                {
                   return code = "PO-000" + max;
                }
                else if(max.ToString().Length == 2)
                {
                    return code = "PO-00" + max;
                }
                else
                {
                    return code ="PO-0"+ max.ToString();
                }
            }
        }
        public ActionResult Request_Line()
        {
            if (IsValid() == true)
            {
                try
                {
                    return View();
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    // Log the error using your existing error handling function
                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "PO_Request", "Request_Line", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public ActionResult SaveOrUpdate(FormCollection collection)
        {

            if (IsValid() == true)
            {
                try
                {
                    //collection data form of string 

                    int header_pk;
                    DateTime purchsedate = Convert.ToDateTime(collection.Get("Request_Header_Date"));
                    string Code = collection.Get("Request_Header_Code");

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                PO_Request_Header header = new PO_Request_Header
                                {
                                    Request_Header_Date = purchsedate,
                                    Request_Header_Code = Code
                                };
                                db.Entry(header).State = System.Data.Entity.EntityState.Added;
                                db.SaveChanges();
                                header_pk = header.Request_Header_Id;


                                string[] Product_id = collection.Get("item.Product_Id").Split(',');
                                string[] Item_id = collection.Get("item.Item_Id").Split(',');
                                string[] Qantity = collection.Get("item.Qty").Split(',');

                                for (int i = 0; i < Product_id.Length; i++)
                                {
                                    int productid = Convert.ToInt32(Product_id[i]);
                                    int itemid = Convert.ToInt32(Item_id[i]);
                                    decimal quantity = Convert.ToDecimal(Qantity[i]);

                                    PO_Request_Line line = new PO_Request_Line
                                    {
                                        Request_Header_Id = header_pk,
                                        Product_Id = productid,
                                        Item_Id = itemid,
                                        Qty = quantity
                                    };
                                    db.Entry(line).State = System.Data.Entity.EntityState.Added;
                                    //db.SaveChanges();
                                    //transaction.Commit();
                                }
                                db.SaveChanges();
                                transaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                                transaction.Rollback();
                                

                            }
                            finally
                            {
                                db.Database.Connection.Close();
                            }
                        }

                    }

                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    // Log the error using your existing error handling function
                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "PO_Request", "SaveOrUpdate", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
           
        }
    }
}