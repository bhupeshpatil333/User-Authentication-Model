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
    public class ItemController : Controller
    {
        // GET: Item
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

        public ActionResult Index()
        {
            if (IsValid() == true)
            {
                try
                {
                    ViewBag.productlist = ProductDDL();
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
                    error.Error_Maintanance(ErrorMessage, "Item", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult PartialItem()
        {
            if (IsValid() == true)
            {
                try
                {
                    ViewBag.productlist = ProductDDL();
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
                    error.Error_Maintanance(ErrorMessage, "Item", "PartialItem", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetItemReport()
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var reportData = db.spGetItemReport(UserId).ToList();
                        return View(reportData);
                    }
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
                    error.Error_Maintanance(ErrorMessage, "Item", "GetItemReport", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }
        public ActionResult GetById(int id)
        {
            if(IsValid() == true)
            {
                try
                {
                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        ViewBag.productlist = ProductDDL();
                        var Data = new Item();
                        Data = db.Items.Where(x => x.Item_Id == id).FirstOrDefault();
                        Item_Model model = new Item_Model()
                        {
                            Item_Id = Data.Item_Id,
                            Item_Name = Data.Item_Name,
                            //Product_Id = Data.Product_Id,
                            UserId = UserId
                        };
                        return PartialView("Index", model);
                    }
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
                    error.Error_Maintanance(ErrorMessage, "Item", "GetById", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult DeleteData(int id)
        {
            if(IsValid() == true)
            {
                try
                {
                    using (Db_Production_Entities _db = new Db_Production_Entities())
                    {
                        Item registration = new Item()
                        {
                            Item_Id = id
                        };
                        _db.Entry(registration).State = System.Data.Entity.EntityState.Deleted;
                        _db.SaveChanges();
                    };

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Item", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Item_Model model)
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.Items.Where
                                        (x => x.Item_Id == model.Item_Id)
                                       select x).Count();
                        if (Idcount == 0)
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Item registration = new Item()
                                {
                                    Item_Id = model.Item_Id,
                                    Item_Name = model.Item_Name,
                                    Product_Id = model.Product_Id,
                                    UserId = UserId

                                };
                                _db.Entry(registration).State = System.Data.Entity.EntityState.Added;
                                _db.SaveChanges();
                            };
                        }
                        //return RedirectToAction("Index");
                        else
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {

                                Item item = new Item()
                                {
                                    Item_Id = model.Item_Id,
                                    Item_Name = model.Item_Name,
                                    Product_Id = model.Product_Id,
                                    UserId = UserId
                                };
                                _db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                _db.SaveChanges();

                            };

                        }
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Item", "SaveOrEdit", Line.ToString(), "");
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
                    productlist.Add(new SelectListItem { Value=item.Product_Id.ToString(), Text=item.Product_Name.ToString() });
                }

            }
            return productlist;
        }

    }
}