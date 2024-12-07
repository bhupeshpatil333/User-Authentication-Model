using Production_ERP1.Db_Context;
using Production_ERP1.ErrorManagement;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class ProductController : Controller
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
        // GET: Product
        public ActionResult Index()
        {
            if (IsValid() == true)
            {
                try
                {

                    return View();
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Product", "Index", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult PartialProduct()
        {
            if (IsValid() == true)
            {
                try
                {

                    return View();
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Product", "PartialProduct", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetProductReport()
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var reportData = db.spGetProductReport(UserId).ToList();
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
                    error.Error_Maintanance(ErrorMessage, "Product", "GetProductReport", Line.ToString(), "");

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
                        var Data = new Product();
                        Data = db.Products.Where(x => x.Product_Id == id).FirstOrDefault();
                        Product_Model model = new Product_Model()
                        {
                            Product_Id = Data.Product_Id,
                            Product_Name = Data.Product_Name,
                            UserId = UserId
                        };
                        return PartialView("Index", model);
                    }
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    // Log the error using your existing error handling function
                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Product", "GetById", Line.ToString(), "");

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
            if (IsValid() == true)
            {
                try
                {
                    using (Db_Production_Entities _db = new Db_Production_Entities())
                    {
                        Product registration = new Product()
                        {
                            Product_Id = id
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
                    error.Error_Maintanance(ErrorMessage, "Product", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Product_Model model)
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.Products.Where
                                        (x => x.Product_Id == model.Product_Id)
                                       select x).Count();
                        if (Idcount == 0)
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Product registration = new Product()
                                {
                                    Product_Id = model.Product_Id,
                                    Product_Name = model.Product_Name,
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

                                Product product = new Product()
                                {
                                    Product_Id = model.Product_Id,
                                    Product_Name = model.Product_Name,
                                    UserId = UserId
                                };
                                _db.Entry(product).State = System.Data.Entity.EntityState.Modified;
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
                    error.Error_Maintanance(ErrorMessage, "Product", "SaveOrEdit", Line.ToString(), "");
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