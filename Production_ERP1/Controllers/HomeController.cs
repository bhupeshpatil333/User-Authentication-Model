using Production_ERP1.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Production_ERP1.Controllers
{
    public class HomeController : Controller
    {
        public bool IsValid()
        {
            if(Session["FirstName"] != null&& Session["LastName"]!=null && Session["EmailId"] != null && Session["UserId"] != null)
            {
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
                    string ErrorMessage = ex.Message;
                    var st = new StackTrace(ex, true);
                    var Frame = st.GetFrame(0);
                    var Line = Frame.GetFileLineNumber();

                    Error_Log_Function error = new Error_Log_Function();
                    error.Error_Maintanance(ErrorMessage, "Home", "Index", Line.ToString(), "");
                    return RedirectToAction("Index", "Error_Page");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult About()
        {
            if (IsValid() == true) {
                ViewBag.Message = "Your application description page.";

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}