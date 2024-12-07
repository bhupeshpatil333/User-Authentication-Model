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
    public class TaxController : Controller
    {
        // GET: Tax
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
                    error.Error_Maintanance(ErrorMessage, "Tax", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult PartialTax()
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
                    error.Error_Maintanance(ErrorMessage, "Tax", "PartialTax", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetTaxReport()
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var reportData = db.spGetTaxReport().ToList();
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
                    error.Error_Maintanance(ErrorMessage, "Tax", "GetTaxReport", Line.ToString(), "");

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
                        var Data = new Tax();
                        Data = db.Taxes.Where(x => x.Tax_Id == id).FirstOrDefault();
                        Tax_Model model = new Tax_Model()
                        {
                            Tax_Id = Data.Tax_Id,
                            Tax_Name = Data.Tax_Name,
                            Tax_Percentage = Data.Tax_Percentage

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
                    error.Error_Maintanance(ErrorMessage, "Tax", "GetById", Line.ToString(), "");

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
                        Tax registration = new Tax()
                        {
                            Tax_Id = id
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
                    error.Error_Maintanance(ErrorMessage, "Tax", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Tax_Model model)
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.Taxes.Where
                                        (x => x.Tax_Id == model.Tax_Id)
                                       select x).Count();
                        if (Idcount == 0)
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Tax registration = new Tax()
                                {
                                    Tax_Id = model.Tax_Id,
                                    Tax_Name = model.Tax_Name,
                                    Tax_Percentage = model.Tax_Percentage

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

                                Tax tax = new Tax()
                                {
                                    Tax_Id = model.Tax_Id,
                                    Tax_Name = model.Tax_Name,
                                    Tax_Percentage = model.Tax_Percentage
                                };
                                _db.Entry(tax).State = System.Data.Entity.EntityState.Modified;
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
                    error.Error_Maintanance(ErrorMessage, "Tax", "SaveOrEdit", Line.ToString(), "");
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