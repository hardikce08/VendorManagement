using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VendorMgmt.Helper
{
    public class Functions
    {
        public static void SendEmail(string receiver, string subject, string EmailMessage,bool IncludeCC)
        {
            try
            {
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(receiver));  // replace with valid value 
                message.From = new MailAddress("info@loan4payday.org");  // replace with valid value
                message.Subject = subject;
                //message.Body = string.Format(body, "Vendor System", "info@vendor.com", "New Registration Added");
                message.Body = EmailMessage;
                message.IsBodyHtml = true;
                if (IncludeCC)
                {
                    message.CC.Add(new MailAddress ("accountvalidation@nsknox.net"));
                    message.CC.Add(new MailAddress("acctspay.dofasco@arcelormittal.com"));
                }
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "info@loan4payday.org",  // replace with valid value
                        Password = "Hardik@123"  // replace with valid value
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = "webmail.loan4payday.org";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
