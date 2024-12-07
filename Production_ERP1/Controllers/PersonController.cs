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
    public class PersonController : Controller
    {
        // GET: Person
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
                    ViewBag.personList = PersonTypeDDL();
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
                    error.Error_Maintanance(ErrorMessage, "Person", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult PartialPerson()
        {
            if (IsValid() == true)
            {
                try
                {
                    ViewBag.personList = PersonTypeDDL();
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
                    error.Error_Maintanance(ErrorMessage, "Person", "Index", Line.ToString(), "");

                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GetPersonReport()
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var reportData = db.spGetPersonReport(UserId).ToList();
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
                    error.Error_Maintanance(ErrorMessage, "Person", "GetPersonReport", Line.ToString(), "");

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
                        ViewBag.personList = PersonTypeDDL();
                        var Data = new Person();
                        Data = db.People.Where(x => x.Person_Id == id).FirstOrDefault();
                        Person_Model model = new Person_Model()
                        {
                            Person_Id = Data.Person_Id,
                            Person_Name = Data.Person_Name,
                            Person_Contact = Data.Person_Contact,
                            PersonType_Id = Data.PersonType_Id,
                            Email_Id = Data.Email_Id,
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
                    error.Error_Maintanance(ErrorMessage, "Person", "GetById", Line.ToString(), "");

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
                        Person registration = new Person()
                        {
                            Person_Id = id
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
                    error.Error_Maintanance(ErrorMessage, "Person", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Person_Model model)
        {
            if (IsValid() == true)
            {
                try
                {

                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.People.Where
                                        (x => x.Person_Id == model.Person_Id)
                                       select x).Count();
                        if (Idcount == 0)
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Person registration = new Person()
                                {
                                    Person_Id = model.Person_Id,
                                    Person_Name = model.Person_Name,
                                    Person_Contact = model.Person_Contact,
                                    PersonType_Id = model.PersonType_Id,
                                    Email_Id = model.Email_Id,
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

                                Person person = new Person()
                                {
                                    Person_Id = model.Person_Id,
                                    Person_Name = model.Person_Name,
                                    Person_Contact = model.Person_Contact,
                                    PersonType_Id = model.PersonType_Id,
                                    Email_Id = model.Email_Id,
                                    UserId = UserId
                                };
                                _db.Entry(person).State = System.Data.Entity.EntityState.Modified;
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
                    error.Error_Maintanance(ErrorMessage, "Person", "SaveOrEdit", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public List<SelectListItem> PersonTypeDDL()
        {
            var personList = new List<SelectListItem>();
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var data = (from x in db.Person_Type
                            select new
                            {
                                x.PersonType_Id,
                                x.Person_Name
                            }).ToList();
                foreach (var item in data)
                {
                    personList.Add(new SelectListItem { Value = item.PersonType_Id.ToString(), Text = item.Person_Name.ToString() });
                }

            }
            return personList;
        }

    }
}