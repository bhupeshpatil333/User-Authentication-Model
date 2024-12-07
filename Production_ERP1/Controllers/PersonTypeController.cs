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
    public class PersonTypeController : Controller
    {
        // GET: PersonType
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult PartialPersonType()
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "PartialPersonType", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetPersonTypeReport()
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var reportData = db.spGetPerson_TypeReport().ToList();
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "GetPersonTypeReport", Line.ToString(), "");

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
                        var Data = new Person_Type();
                        Data = db.Person_Type.Where(x => x.PersonType_Id == id).FirstOrDefault();
                        Person_Type_Model model = new Person_Type_Model()
                        {
                            PersonType_Id = Data.PersonType_Id,
                            Person_Name = Data.Person_Name
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "GetById", Line.ToString(), "");

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
                        Person_Type registration = new Person_Type()
                        {
                            PersonType_Id = id
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Person_Type model)
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.Person_Type.Where
                                        (x => x.PersonType_Id == model.PersonType_Id)
                                       select x).Count();
                        if (Idcount == 0)
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Person_Type registration = new Person_Type()
                                {
                                    PersonType_Id = model.PersonType_Id,
                                    Person_Name = model.Person_Name

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

                                Person_Type personType = new Person_Type()
                                {
                                    PersonType_Id = model.PersonType_Id,
                                    Person_Name = model.Person_Name
                                };
                                _db.Entry(personType).State = System.Data.Entity.EntityState.Modified;
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
                    error.Error_Maintanance(ErrorMessage, "PersonType", "SaveOrEdit", Line.ToString(), "");
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