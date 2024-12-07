using Production_ERP1.Db_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Production_ERP1.EmailConfig
{
    public class EmailFunctions
    {
        public void SendMail(string To, string Subject, string Body)
        {
            using (Db_Production_Entities db = new Db_Production_Entities())
            {
                var Email_Confog_Data = (from x in db.EmailConfigs select x).FirstOrDefault();

                SmtpClient smtp = new SmtpClient()
                {
                    Port = Convert.ToInt32(Email_Confog_Data.Config_port),
                    Host = Email_Confog_Data.Config_host,
                    Credentials = new NetworkCredential(Email_Confog_Data.from_mail_id, Email_Confog_Data.from_password),
                    EnableSsl = true
                };
                MailMessage message = new MailMessage(Email_Confog_Data.from_mail_id, To)
                {
                    Subject = Subject,
                    Body = Body
                };
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}