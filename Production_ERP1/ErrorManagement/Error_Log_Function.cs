using Production_ERP1.Db_Context;
using Production_ERP1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Production_ERP1.ErrorManagement
{
    public class Error_Log_Function
    {
        public void Error_Maintanance(string Error_Message, string Controller_Name, string Function, string Line, string Data)
        {

            using (Db_Production_Entities db = new Db_Production_Entities())
            {


                ErrorLog _log = new ErrorLog()
                {
                    ErrorId = 0,
                    ErrorMessage = Error_Message,
                    ErrorController = Controller_Name,
                    ErrorFunction = Function,
                    ErrorLine = Line,
                    ErrorData = Data

                };
                db.Entry(_log).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

            }
        }
    }
}