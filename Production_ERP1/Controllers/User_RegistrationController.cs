using Production_ERP1.Db_Context;
using Production_ERP1.EmailConfig;
using Production_ERP1.ErrorManagement;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class User_RegistrationController : Controller
    {
        public bool IsValid()
        {
            if (Session["FirstName"] != null && Session["LastName"] != null && Session["EmailId"] != null && Session["UserId"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // GET: User_Registration
        public ActionResult Index()
        {
           
                try
                {
                    //int a = 10;
                    //int b = a / 0;
                    return View();
                }
                catch (Exception ex)
                {
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "User_Registration", "Index", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
        }
        public ActionResult UserPartial()
        {
            return View();
        }
        public ActionResult SignUp(User_Registration_Model model)
        {
            //if(IsValid() == true)
            //{
                using (Db_Production_Entities db = new Db_Production_Entities())
                {
                    try
                    {
                        //int a = 10;
                        //int b = a / 0;
                        // *************** if the user is new then count is 0 *********************************

                        // Linq
                        var emailcount = (from x in db.User_Registration.Where
                                        (x => x.Email_Id == model.Email_Id)
                                          select x).Count();

                        if (emailcount == 0)
                        {

                            if (model.Image != null)
                            {
                                string imagename = Path.GetFileNameWithoutExtension(model.Image.FileName);
                                imagename = imagename + System.DateTime.Now.ToString("yymmssff");
                                string imageextention = Path.GetExtension(model.Image.FileName);
                                imagename = imagename + imageextention;
                                model.Profile_Image = "~/Image/ProfileImage" + imagename;
                                imagename = Path.Combine(Server.MapPath("~/Image/ProfileImage"), imagename);
                                model.Image.SaveAs(imagename);


                            }
                            else
                            {
                                TempData["ErrorImage"] = "Please Upload Image";
                                return RedirectToAction("UserPartial");
                            }


                            var EmailTemplate = (from x in db.EmailTemplates.Where(x => x.template_id == 1) select x).FirstOrDefault();

                            int minRange = 0000;
                            int maxRange = 9999;
                            //******************************* Random OTP *****************************
                            Random R = new Random();
                            int OTP = R.Next(minRange, maxRange);

                            string OtpMessage = "\n \n Your OTP is: " + OTP.ToString();

                            string Recipt = "\n\n Dear " + model.FirstName + " " + model.LastName + " \n";

                            string Body = Recipt + EmailTemplate.template_body.ToString() + OtpMessage;

                            EmailFunctions Email = new EmailFunctions();
                            Email.SendMail(model.Email_Id, EmailTemplate.template_subject, Body);

                            // *************************** insert  the values  in Database ************************************* 
                            using (Db_Production_Entities _db = new Db_Production_Entities())
                            {
                                User_Registration registration = new User_Registration()
                                {
                                    Email_Id = model.Email_Id,
                                    User_Password = OTP.ToString(),
                                    FirstName = model.FirstName,
                                    LastName = model.LastName,
                                    Profile_Image = model.Profile_Image,
                                    Created_Date = System.DateTime.Now,
                                    Verify = "N"
                                };
                                _db.Entry(registration).State = System.Data.Entity.EntityState.Added;
                                _db.SaveChanges();
                            };
                            // 

                        }
                        TempData["All_FieldRequired"] = "Please Fill All Details";
                        //return RedirectToAction("Index");

                        //return RedirectToAction("Index", "User_Registration");
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        var st = new StackTrace(ex, true);
                        var Frame = st.GetFrame(0);
                        var Line = Frame.GetFileLineNumber();

                        Error_Log_Function error = new Error_Log_Function();
                        error.Error_Maintanance(ErrorMessage, "User_Registration", "SignUp", Line.ToString(), "");
                        return RedirectToAction("Index", "Error_Page");
                    }
                    return RedirectToAction("Index", "Login");
                }
                
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Login");
            //}
        }
    }
}