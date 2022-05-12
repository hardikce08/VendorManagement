using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VendorMgmt.Helper
{
    public class Functions
    {
        public static void SendEmail_Old(string receiver, string subject, string EmailMessage,bool IncludensKnox)
        {
            try
            {
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(receiver));  // replace with valid value 
                message.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"]);  // replace with valid value
                message.Subject = subject;
                //message.Body = string.Format(body, "Vendor System", "info@vendor.com", "New Registration Added");
                message.Body = EmailMessage;
                message.IsBodyHtml = true;
                if (IncludensKnox)
                {
                    foreach (string s in ConfigurationManager.AppSettings["nsKnoxEmail"].Split(';'))
                    {
                        message.CC.Add(new MailAddress(s));
                    }
                }
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = ConfigurationManager.AppSettings["EmailUserName"],  // replace with valid value
                        Password = ConfigurationManager.AppSettings["EmailPassword"] // replace with valid value
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = ConfigurationManager.AppSettings["EmailHost"];
                    smtp.Port = Convert.ToInt32 (ConfigurationManager.AppSettings["Port"]);
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void SendEmail(string receiver, string subject, string EmailMessage, bool IncludensKnox)
        {
            try
            {
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                //foreach (string s in receiver.Split(';'))
                //{
                //    message.To.Add(new MailAddress(s));
                //}
                message.To.Add(new MailAddress("hardikce.08@gmail.com"));  // replace with valid value 
                message.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["FromEmailDisplayName"]);  // replace with valid value
                message.Subject = subject;
                //message.Body = string.Format(body, "Vendor System", "info@vendor.com", "New Registration Added");
                message.Body = EmailMessage;
                message.IsBodyHtml = true;
                if (IncludensKnox)
                {
                    foreach (string s in ConfigurationManager.AppSettings["nsKnoxEmail"].Split(';'))
                    {
                        message.CC.Add(new MailAddress(s));
                    }
                }
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = ConfigurationManager.AppSettings["EmailUserName"],  // replace with valid value
                        Password = ConfigurationManager.AppSettings["EmailPassword"] // replace with valid value
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = ConfigurationManager.AppSettings["EmailHost"];
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                    smtp.EnableSsl = true;
                    smtp.TargetName = "STARTTLS/smtp.office365.com"; // Set to avoid MustIssueStartTlsFirst exception
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
