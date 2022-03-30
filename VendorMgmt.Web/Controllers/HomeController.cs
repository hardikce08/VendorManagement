using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorMgmt.DataAccess;
using VendorMgmt.DataAccess.Model;
using VendorMgmt.Helper;

namespace VendorMgmt.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                var parameters = new Dictionary<string, string>
                {
                    { "response_mode", "form_post" },
                     { "response_type", "token" },
                    { "client_id", System.Configuration.ConfigurationManager.AppSettings["ClientId"] },
                    { "redirect_uri", System.Configuration.ConfigurationManager.AppSettings["redirectUri"] },
                    { "prompt", "login"},
                    {"nonce","678910" },
                      {"state","12345" },
                    { "scope", "openid"}
                };

                var requestUrl = string.Format("{0}/authorize?{1}", EndPointUrl, BuildQueryString(parameters));

                Response.Redirect(requestUrl);
            }
            return View();

        }
        public ActionResult Dashboard(VendorMaster model)
        {
            if (Request.Cookies["UserToken"] != null)
            {
                //You get the user's first and last name below:
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.HttpMethod == "POST" && !string.IsNullOrEmpty(model.BusinessName))
            {
                VendorService vs = new VendorService();
                //model.RegistrationCode = Request["RegistrationCode"];
                //send email function 
                string Emailbody = "";
                if (model.nsKnox)
                {
                    Emailbody = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/nsknox.html"));
                }
                else
                { Emailbody = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/normal.html")); }

                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/FillInfo/"+model.RegistrationCode);
                Emailbody = Emailbody.Replace("{CompanyName}", model.BusinessName);
                Emailbody = Emailbody.Replace("{RegCode}", model.RegistrationCode);
                Emailbody = Emailbody.Replace("{DofascoContact}", model.DofascoEmail);
                Functions.SendEmail("hardikce.08@gmail.com", "New Vendor Added", Emailbody , model.nsKnox);
                vs.VendorMaster_InsertOrUpdate(model);
                ViewBag.JavaScriptFunction = "successToast();";
                model = new VendorMaster();
               
            }
            model.RegistrationCode = CommonHelper.GetRegistrationCode();
            return View(model);
        }
        public ActionResult Vendors()
        {
            if (Request.Cookies["UserToken"] != null)
            {
                //You get the user's first and last name below:
                ViewBag.Name = Request.Cookies["UserName"]?.Value;
                ViewBag.UserGuid = Request.Cookies["UserGuid"]?.Value;
                // The 'preferred_username' claim can be used for showing the username
                ViewBag.Username = Request.Cookies["UserEmail"]?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
             
            return View();
        }
        [HttpPost]
        public JsonResult GenerateNewNumber()
        {

            return Json(new { RegistrationCode = CommonHelper.GetRegistrationCode() }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public JsonResult SuggestBusinessName(string name)
        //{
            
        //    //Note : you can bind same list from database  
        //    List<string> ObjList = vs.VendorMasters.Select(p => p.BusinessName).ToList();
        //    //Searching records from list using LINQ query  
        //    var Name = (from N in ObjList
        //                where N.StartsWith(name)
        //                select new { N });
        //    return Json(Name.ToList(), JsonRequestBehavior.AllowGet);

        //}
        public JsonResult GetList(string name)
        {
            VendorService vs = new VendorService();
            var list = vs.VendorMasters.Where(x => x.BusinessName.StartsWith(name)).Take(10).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CallBack()
        {
            if (Request.Params["access_token"] != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(Request.Params["access_token"]);
                var userClaims = decodedValue.Payload;
                //ViewBag.Name = userClaims["name"]?.Value;
                //ViewBag.UserGuid = userClaims["aud"]?.Value;
                //ViewBag.Username = userClaims["unique_name"]?.Value;
                DateTime expiredate = DateTime.Now.AddHours(12);
                var UserToken = new HttpCookie("UserToken");
                UserToken.Expires = expiredate;
                UserToken.Value = Request.Params["access_token"].ToString();
                HttpContext.Response.Cookies.Add(UserToken);
                var UserEmail = new HttpCookie("UserEmail");
                UserEmail.Expires = expiredate;
                UserEmail.Value = userClaims["unique_name"]?.ToString();
                HttpContext.Response.Cookies.Add(UserEmail);
                var UserName = new HttpCookie("UserName");
                UserName.Expires = expiredate;
                UserName.Value = userClaims["name"]?.ToString();
                HttpContext.Response.Cookies.Add(UserName);
                var UserGuid = new HttpCookie("UserGuid");
                UserGuid.Expires = expiredate;
                UserGuid.Value = userClaims["aud"]?.ToString();
                HttpContext.Response.Cookies.Add(UserGuid);
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        protected string EndPointUrl
        {
            get
            {
                return string.Format("{0}/{1}/{2}", "https://login.microsoftonline.com", "common", @"oauth2/v2.0");
            }
        }
        private string BuildQueryString(IDictionary<string, string> parameters)
        {
            var list = new List<string>();

            foreach (var parameter in parameters)
            {
                list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
            }

            return string.Join("&", list);
        }
        public static string SiteUrl
        {
            get
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
                    return string.Empty;

                var request = System.Web.HttpContext.Current.Request;
                return request.Url.Scheme + "://" + request.Url.DnsSafeHost + (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port.ToString());
            }
        }
    }
}