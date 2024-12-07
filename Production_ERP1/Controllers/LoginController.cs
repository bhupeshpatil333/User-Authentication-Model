using Production_ERP1.Db_Context;
using Production_ERP1.EmailConfig;
using Production_ERP1.ErrorManagement;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class LoginController : Controller
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
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OtpPage()
        {
            if (IsValid() == true)
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
                    error.Error_Maintanance(ErrorMessage, "Login", "OtpPage", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult RegenerateOtp(User_Registration_Model model)
        {
            //if(IsValid() == true)
            //{
                using (Db_Production_Entities db = new Db_Production_Entities())
                {
                    try
                    {
                        // ********************* if the user is exist then the OTP is Regenerate and Update the OTP ********************
                        var emailcount = (from x in db.User_Registration.Where
                                        (x => x.Email_Id == model.Email_Id)
                                          select x).Count();

                        if (emailcount > 0)
                        {

                            var EmailTemplate = (from x in db.EmailTemplates.Where(x => x.template_id == 1) select x).FirstOrDefault();

                            int minRange = 0000;
                            int maxRange = 9999;

                            Random R = new Random();
                            int OTP = R.Next(minRange, maxRange);

                            string OtpMessage = "\n \n Your OTP is: " + OTP.ToString();

                            string Recipt = "\n\n Dear " + model.FirstName + " " + model.LastName + " \n";

                            string Body = Recipt + EmailTemplate.template_body.ToString() + OtpMessage;

                            EmailFunctions Email = new EmailFunctions();
                            Email.SendMail(model.Email_Id, EmailTemplate.template_subject, Body);

                            // UpdateOtp is StoreProcedure to Save the changes apply in DataBase
                            db.UpdateOtp(model.Email_Id, OTP.ToString());

                            return RedirectToAction("Index", "Login");


                        }
                        else
                        {

                            return RedirectToAction("Index", "Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        var st = new StackTrace(ex, true);
                        var Frame = st.GetFrame(0);
                        var Line = Frame.GetFileLineNumber();

                        Error_Log_Function error = new Error_Log_Function();
                        error.Error_Maintanance(ErrorMessage, "Login", "RegenerateOtp", Line.ToString(), "");
                        return RedirectToAction("Index", "Error_Page");
                    }


                }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            
        }
        public ActionResult ChangePasswordPage()
        {
            if (IsValid() == true)
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
                    error.Error_Maintanance(ErrorMessage, "Login", "ChangePasswordPage", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ChangePassword(User_Registration_Model model)
        {
            //if (IsValid() == true)
            //{
                using (Db_Production_Entities db = new Db_Production_Entities())
                {
                    try
                    {
                        var Changepasswod = (from x in db.User_Registration.Where(x => x.UserId == model.UserId) select x).FirstOrDefault();

                        using (Db_Production_Entities _db = new Db_Production_Entities())
                        {
                            User_Registration U = new User_Registration()
                            {
                                UserId = Changepasswod.UserId,
                                Email_Id = Changepasswod.Email_Id,
                                Profile_Image = Changepasswod.Profile_Image,
                                Created_Date = System.DateTime.Now,
                                FirstName = Changepasswod.FirstName,
                                LastName = Changepasswod.LastName,
                                User_Password = model.User_Password,
                                Verify = "Y"
                            };
                            _db.Entry(U).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        var st = new StackTrace(ex, true);
                        var Frame = st.GetFrame(0);
                        var Line = Frame.GetFileLineNumber();

                        Error_Log_Function error = new Error_Log_Function();
                        error.Error_Maintanance(ErrorMessage, "Login", "ChangePassword", Line.ToString(), "");
                        return RedirectToAction("Index", "Error_Page");
                    }

                }
                return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Login");
            //}
        }
        public ActionResult CheckAuthentication(User_Registration_Model model)
        {
            //if(IsValid() == true)
            //{
                using (Db_Production_Entities db = new Db_Production_Entities())
                {
                    try
                    {
                        // new User_Registration here.......

                        var checkAuthentication = (from x in db.User_Registration.Where(x =>
                        x.Email_Id == model.Email_Id && x.User_Password == model.User_Password && x.Verify == "N")
                                                   select x).FirstOrDefault();

                        if (checkAuthentication != null)
                        {
                            DateTime createdDate = Convert.ToDateTime(checkAuthentication.Created_Date);
                            DateTime currentDateTime = System.DateTime.Now;

                            // Subtract createdDate from currentDateTime
                            TimeSpan difference = currentDateTime.Subtract(createdDate);

                            //difference of Miniuts
                            int min = difference.Minutes;

                            // if user is login within 5 miniuts then

                            if (min <= 5)
                            {
                                //validate  5 miniuts
                                Session.Add("UserId", checkAuthentication.UserId);

                                Session.Add("FirstName", checkAuthentication.FirstName);
                                Session.Add("LastName", checkAuthentication.LastName);
                                Session.Add("EmailId", checkAuthentication.Email_Id);
                                //checkAuthentication.Verify = "Y";
                                //db.SaveChanges();

                                using (Db_Production_Entities database = new Db_Production_Entities())
                                {
                                    // ***************** If the Email Verification done withing 5 Miniuts the it Verify = Y ***********************
                                    User_Registration U = new User_Registration()
                                    {
                                        UserId = checkAuthentication.UserId,
                                        Email_Id = checkAuthentication.Email_Id,
                                        Profile_Image = checkAuthentication.Profile_Image,
                                        Created_Date = System.DateTime.Now,
                                        FirstName = checkAuthentication.FirstName,
                                        LastName = checkAuthentication.LastName,
                                        User_Password = checkAuthentication.User_Password,
                                        Verify = "Y"
                                    };
                                    database.Entry(U).State = System.Data.Entity.EntityState.Modified;
                                    database.SaveChanges();
                                }
                                // Change Password
                                TempData["TimeExpire"] = "Please Regenrate OTP Time Expire";

                                TempData["UserID"] = checkAuthentication.UserId;

                            // ********* if the verification not done then it will redirect to ChangePasswordPage Page **************
                            return RedirectToAction("ChangePasswordPage");
                            }
                            else
                            {
                            
                            TempData["Email_Id"] = checkAuthentication.Email_Id;
                                // ************** if user is login greater than 5 miniuts then Redirect to OtpPage in Login Controller ************
                                return RedirectToAction("OtpPage");
                            }
                        }
                        else
                        {

                            var Authdadication = (from x in db.User_Registration.Where(x =>
                            x.Email_Id == model.Email_Id && x.User_Password == model.User_Password && x.Verify == "Y")
                                                  select x).FirstOrDefault();
                            if (Authdadication != null)
                            {
                                //Session
                                Session.Add("UserId", Authdadication.UserId);
                                Session.Add("FirstName", Authdadication.FirstName);
                                Session.Add("LastName", Authdadication.LastName);
                                Session.Add("EmailId", Authdadication.Email_Id);
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UserIssue"] = "Please Check User Name and Pasword";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = ex.Message;
                        var st = new StackTrace(ex, true);
                        var Frame = st.GetFrame(0);
                        var Line = Frame.GetFileLineNumber();

                        Error_Log_Function error = new Error_Log_Function();
                        error.Error_Maintanance(ErrorMessage, "Login", "CheckAuthentication", Line.ToString(), "");
                        return RedirectToAction("Index", "Error_Page");
                    }

                }
                //  ********* if the User is Not Registred or Wrong Email then it will redirect to UserPartial Page in User_Registration Controller **************
                return RedirectToAction("UserPartial", "User_Registration");
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Login");
            //}
        }
    }
}