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
using static System.Net.WebRequestMethods;

namespace Production_ERP1.Controllers
{
    public class Client_RegistrationController : Controller, IClient_RegistrationController
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
        // GET: Client_Registration
        public ActionResult Index()
        {
            if (IsValid() == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
          
        }
        public ActionResult PartialClient()
        {
            if (IsValid() == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        public ActionResult GetReport()
        {
            if (IsValid() == true)
            {
                try
                {
                
                using (Db_Production_Entities db = new Db_Production_Entities())
                    {

                        var reportData = db.spGetClientReports(UserId).ToList();

                        
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
                    error.Error_Maintanance(ErrorMessage, "Client_Registration", "GetReport", Line.ToString(), "");

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
            if (IsValid() == true)
            {
                using (Db_Production_Entities db = new Db_Production_Entities())
                {
                    var Data = new Client_Registration();
                    Data = db.Client_Registration.Where(x => x.ClientId == id).FirstOrDefault();
                    Client_Registration_Model model = new Client_Registration_Model()
                    {
                        ClientId = Data.ClientId,
                        ClientName = Data.ClientName,
                        ClientBusiness_Name = Data.ClientBusiness_Name,
                        Client_Logo = Data.Client_Logo,
                        Client_GSTNo = Data.Client_GSTNo,
                        Client_GST_Document = Data.Client_GST_Document,
                        UserId = UserId,
                        Client_Contact = Data.Client_Contact
                    };
                    return PartialView("Index", model);
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
                        Client_Registration registration = new Client_Registration()
                        {
                            ClientId= id       
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
                    error.Error_Maintanance(ErrorMessage, "Client_Registration", "DeleteData", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult SaveOrEdit(Client_Registration_Model model)
        {
            if (IsValid() == true)
            {
                try
                {
                    
                    using (Db_Production_Entities db = new Db_Production_Entities())
                    {
                        var Idcount = (from x in db.Client_Registration.Where
                                        (x => x.ClientId == model.ClientId)
                                       select x).Count();
                        if (Idcount == 0)
                        {

                            if (model.Logo != null && model.Documennt != null)
                            {
                                string imagename = Path.GetFileNameWithoutExtension(model.Logo.FileName);
                                imagename = imagename + System.DateTime.Now.ToString("yymmssff");
                                string imageextention = Path.GetExtension(model.Logo.FileName);
                                imagename = imagename + imageextention;
                                model.Client_Logo = "~/Image/Logo" + imagename;
                                imagename = Path.Combine(Server.MapPath("~/Image/Logo"), imagename);
                                model.Logo.SaveAs(imagename);

                                string documentName = Path.GetFileNameWithoutExtension(model.Documennt.FileName);
                                documentName = documentName + System.DateTime.Now.ToString("yymmssff");
                                string Documentextention = Path.GetExtension(model.Documennt.FileName);
                                documentName = documentName + Documentextention;
                                model.Client_GST_Document = "~/Image/Document" + documentName;
                                documentName = Path.Combine(Server.MapPath("~/Image/Document"), documentName);
                                model.Documennt.SaveAs(documentName);

                                using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                Client_Registration registration = new Client_Registration()
                                {
                                    ClientId =model.ClientId,
                                    ClientName = model.ClientName,
                                    ClientBusiness_Name = model.ClientBusiness_Name,
                                    Client_Logo = imagename,
                                    Client_GSTNo = model.Client_GSTNo,
                                    Client_GST_Document = documentName,
                                    UserId = UserId,
                                    Client_Contact = model.Client_Contact

                                };
                                _db.Entry(registration).State = System.Data.Entity.EntityState.Added;
                                _db.SaveChanges();
                            };

                            }
                            else
                            {
                                TempData["ErrorImage"] = "Please Upload Image";
                                TempData["ErrorDocument"] = "Please Upload Document?";
                                return RedirectToAction("PartialClient");
                            }
                            

                        }
                        //return RedirectToAction("Index");
                        else
                        {
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                if (model.Logo != null && model.Documennt != null)
                                {
                                    string imagename = Path.GetFileNameWithoutExtension(model.Logo.FileName);
                                    imagename = imagename + System.DateTime.Now.ToString("yymmssff");
                                    string imageextention = Path.GetExtension(model.Logo.FileName);
                                    imagename = imagename + imageextention;
                                    model.Client_Logo = "~/Image/Logo" + imagename;
                                    imagename = Path.Combine(Server.MapPath("~/Image/Logo"), imagename);
                                    model.Logo.SaveAs(imagename);

                                    string documentName = Path.GetFileNameWithoutExtension(model.Documennt.FileName);
                                    documentName = documentName + System.DateTime.Now.ToString("yymmssff");
                                    string Documentextention = Path.GetExtension(model.Documennt.FileName);
                                    documentName = documentName + Documentextention;
                                    model.Client_GST_Document = "~/Image/Document" + documentName;
                                    documentName = Path.Combine(Server.MapPath("~/Image/Document"), documentName);
                                    model.Documennt.SaveAs(documentName);

                                }
                                Client_Registration client = new Client_Registration()
                                {
                                    ClientId = model.ClientId,
                                    ClientName = model.ClientName,
                                    ClientBusiness_Name = model.ClientBusiness_Name,
                                    Client_Logo = model.Client_Logo,
                                    Client_GSTNo = model.Client_GSTNo,
                                    Client_GST_Document = model.Client_GST_Document,
                                    UserId = UserId,
                                    Client_Contact = model.Client_Contact
                                };
                                _db.Entry(client).State = System.Data.Entity.EntityState.Modified;
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
                    error.Error_Maintanance(ErrorMessage, "Client_Registration", "SaveOrEdit", Line.ToString(), "");
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