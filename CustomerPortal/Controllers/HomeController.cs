using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using VendorMgmt.DataAccess;
using VendorMgmt.DataAccess.Model;
using VendorMgmt.Helper;
using CacheItemPriority = System.Web.Caching.CacheItemPriority;

namespace CustomerPortal.Controllers
{
    public class HomeController : Controller
    {
        public VendorService vs = new VendorService();
        public VendorService Customervs = new VendorService("s");
        [Route("/FillInfo")]
        public ActionResult FillInfo(VendorFillInfo model, string LinkGuid = "")
        {
            if (LinkGuid == string.Empty)
            {
                LinkGuid = Request.QueryString["id"];
            }
            if (LinkGuid == string.Empty)
            {
                TempData["Error"] = "No Registration Code Provided";
            }
            else
            {

                var VendorMst = vs.VendorMasters.Where(p => p.LinkGuid == LinkGuid && p.LinkExpired == false).FirstOrDefault();
                if (VendorMst == null)
                {
                    TempData["Error"] = "Invalid Registration Code or Registration Code Expired";
                    return View(model);
                }

                model.RegistrationCode = VendorMst.RegistrationCode;
                model.BusinessName = VendorMst.BusinessName;
                model.VendorEmail = VendorMst.VendorEmail;
                model.DofascoEmail = VendorMst.DofascoEmail;
                model.LinkGuid = LinkGuid;
                var Basicinfo = Customervs.VendorBasicInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.BasicInfo = Basicinfo == null ? new VendorBasicInfo() : Basicinfo;

                var FinancialInfo = Customervs.VendorFinancialInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.FinancialInfo = FinancialInfo == null ? new VendorFinancialInfo() : FinancialInfo;

                var PrimarySalesInfo = Customervs.VendorPrimarySalesInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.PrimarySalesInfo = PrimarySalesInfo == null ? new VendorPrimarySalesInfo() : PrimarySalesInfo;

                var SubmittedByInfo = Customervs.VendorSubmittedByInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.SubmittedByInfo = SubmittedByInfo == null ? new VendorSubmittedByInfo() : SubmittedByInfo;

                var RemittanceInfo = Customervs.VendorRemittanceInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.RemittanceInfo = RemittanceInfo == null ? new VendorRemittanceInfo() : RemittanceInfo;

                var BankingInfo = Customervs.VendorBankingInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.BankingInfo = BankingInfo == null ? new VendorBankingInfo() : BankingInfo;

                List<VendorAttachmentInfo> LstAttachment = Customervs.VendorAttachmentInfos.Where(p => p.VendorId == VendorMst.Id).ToList();
                model.AttachmentInfo = LstAttachment;

                var PurchaseInfo = Customervs.VendorPurchasingInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.PurchaseInfo = PurchaseInfo == null ? new VendorPurchasingInfo() : PurchaseInfo;

                var WorkFlowInfo = Customervs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.WorkFlowInfo = WorkFlowInfo == null ? new VendorWorkFlowInfo() : WorkFlowInfo;

                var TreasuryInfo = Customervs.VendorTreasuryInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.TreasuryInfo = TreasuryInfo == null ? new VendorTreasuryInfo() : TreasuryInfo;

                model.VendorId = VendorMst.Id;
                model.lstCountries = vs.GetAllCountry();
                ViewData["SpanTreeLevel1"] = new SelectList(Customervs.SpanTreeLevels.Where(p => p.LevelId == 1).ToList(), "LevelCode", "LevelDescription");
                vs.SetLinkExpired(VendorMst.Id);
                Customervs.VendorMasterCustomer_InsertOrUpdate(VendorMst);
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult ValidateForm(VendorFillInfo model)
        {
            var VendorMst = vs.VendorMasters.Where(p => p.RegistrationCode == model.RegistrationCode).FirstOrDefault();

            if (model.UserRegistrationCode == VendorMst.RegistrationCode && model.Email.ToLower() == VendorMst.VendorEmail.ToLower() && model.CompanyName.ToLower() == VendorMst.BusinessName.ToLower())
            {
                return Json(new { data = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "Authentication Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveTab1(VendorFillInfo model)
        {
            if (model.RegistrationCode == string.Empty)
            {
                TempData["Error"] = "No Registration Code Provided";
            }
            else
            {
                var VendorMst = vs.VendorMasters.Where(p => p.RegistrationCode == model.RegistrationCode).FirstOrDefault();
                if (VendorMst == null)
                {
                    TempData["Error"] = "Invalid Registration Code";
                }
                //var Basicinfo = vs.VendorBasicInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                //model = new VendorFillInfo();
                //model.BasicInfo = Basicinfo == null ? new VendorBasicInfo() : Basicinfo;
                model.BasicInfo.VendorId = model.VendorId;
                model.FinancialInfo.VendorId = model.VendorId;
                //TryUpdateModel<VendorBasicInfo>(model.BasicInfo, new string[] { "VendorName", "AlternateName", "Name", "Signee_Name", "Signee_Phone", "Signee_Email", "OperationsContact", "OperationsPhone", "OperationsEmail", "GlobalAddressBook", "DisplayUserHubProfile", "AccountGroup" });
                //vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                model.PrimarySalesInfo.VendorId = model.VendorId;
                model.BasicInfo.Country = Request["BasicInfo_Country"];
                Customervs.VendorPrimarySalesInfo_InsertOrUpdate(model.PrimarySalesInfo);
                Customervs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                Customervs.VendorFinancialInfo_InsertOrUpdate(model.FinancialInfo);
                return Json(new { PrimarySalesInfoId = model.PrimarySalesInfo.Id, BasicInfoId = model.BasicInfo.Id, FinancialInfoId = model.FinancialInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab2(VendorFillInfo model)
        {
            if (model.RegistrationCode == string.Empty)
            {
                TempData["Error"] = "No Registration Code Provided";
            }
            else
            {
                var VendorMst = vs.VendorMasters.Where(p => p.RegistrationCode == model.RegistrationCode).FirstOrDefault();
                if (VendorMst == null)
                {
                    TempData["Error"] = "Invalid Registration Code";
                }
                model.SubmittedByInfo.VendorId = model.VendorId;
                Customervs.VendorSubmittedByInfo_InsertOrUpdate(model.SubmittedByInfo);
                model.RemittanceInfo.VendorId = model.VendorId;
                model.RemittanceInfo.Country = Request["RemittanceInfo_Country"];
                Customervs.VendorRemittanceInfo_InsertOrUpdate(model.RemittanceInfo);

                model.BankingInfo.VendorId = model.VendorId;
                model.BankingInfo.Country = Request["BankingInfo_Country"];
                Customervs.VendorBankingInfo_InsertOrUpdate(model.BankingInfo);

                //send Email to Vendor for Notification about Complete Form 
                string Emailbody = "";
                EmailTemplateService es = new EmailTemplateService();
                Emailbody = es.EmailTemplateByName("VendorConfirmation").EmailBody;
                Emailbody = Emailbody.Replace("@VendorName", model.BusinessName);
                Functions.SendEmail(model.VendorEmail, "Confirmation", Emailbody, false);

                //Send Notification to Dofasco email that User filled form
                Emailbody = es.EmailTemplateByName("DofascoConfirmation").EmailBody;
                Emailbody = Emailbody.Replace("@VendorName", model.BusinessName);
                Emailbody = Emailbody.Replace("{ShortUrl}", AdminPortalURL + "/Vendor/GetDetails?RegistrationCode=" + model.RegistrationCode);
                Functions.SendEmail(model.DofascoEmail, "Confirmation", Emailbody, false);
                //update Application Status
                vs.UpdateStatus(model.RegistrationCode, "Submitted by Vendor");


                return Json(new { BankingInfoId = model.BankingInfo.Id,  SubmittedByInfoId = model.SubmittedByInfo.Id, RemittanceInfoId = model.RemittanceInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { BankingInfoId = 0, PrimarySalesInfoId = 0, SubmittedByInfoId = 0, RemittanceInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadFiles()
        {
            string VendorId = Request["VendorId"];
            //Save Files if its Exist 
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                int VendorID = Convert.ToInt32(VendorId);
                // Remove Existing files 
                List<VendorAttachmentInfo> LstAttachment = Customervs.VendorAttachmentInfos.Where(p => p.VendorId == VendorID).ToList();
                foreach (var item in LstAttachment)
                {
                    var obj = Customervs.VendorAttachmentInfos.Where(p => p.Id == item.Id).FirstOrDefault();
                    string FilePath = Path.Combine(Server.MapPath("~/Attachments/"), VendorID.ToString() + "_" + obj.FileName);
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    Customervs.VendorAttachmentInfo_Remove(obj);
                }


                HttpPostedFileBase file = files[i];
                string fname;
                // Checking for Internet Explorer      
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                fname = Path.Combine(Server.MapPath("~/Attachments/"), fname);

                VendorAttachmentInfo t = new VendorAttachmentInfo();
                t.FileName = Path.GetFileName(fname);
                t.FileType = Path.GetExtension(fname);
                t.VendorId = Convert.ToInt32(VendorId);
                Customervs.VendorAttachmentInfo_InsertOrUpdate(t);
                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Attachments/"), t.VendorId.ToString() + "_" + t.FileName);
                file.SaveAs(fname);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferData()
        {
            string VendorId = Request["VendorId"];
            //Transfer Data from Customer to Admin Portal DB
            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => new Worker().StartProcessing(Convert.ToInt32(VendorId), cancellationToken));
            //Transfer Data from Customer to Admin Portal DB
            TransferFiles();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Success()
        {
            return View();
        }
        public void TransferFiles()
        {
            using (var client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    string[] AllfileNames = Directory.GetFiles(Server.MapPath("~/Attachments/"));
                    foreach (var file in AllfileNames)
                    {
                        var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(file));
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = Path.GetFileName(file)
                        };
                        formData.Add(fileContent);

                        try
                        {
                            var response = client.PostAsync( AdminPortalURL +"/Home/SaveFile", formData).Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            // log error  
                        }
                    }
                }
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveFile()
        {
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using  
                    // the original file.part name posted  
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath("~/Attachments1/");
                    string path = Path.Combine(UploadPath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                    catch (IOException ex)
                    {
                        // handle  
                    }
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("File uploaded.")
            };
        }
        public ActionResult RemoveFiles(string Id)
        {
            int ID = Convert.ToInt32(Id);
            var obj = Customervs.VendorAttachmentInfos.Where(p => p.Id == ID).FirstOrDefault();
            Customervs.VendorAttachmentInfo_Remove(obj);
            return Content("test");
        }
        public JsonResult ReturnJSONDataToAJax(string ParentCode, int LevelId) //It will be fired from Jquery ajax call
        {

            var jsonData = Customervs.SpanTreeLevels.Where(p => p.LevelId == LevelId && p.ParentLevelText == ParentCode).ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public static string AdminPortalURL
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AdminPortalURL"];
            }
        }

    }
}