using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
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

namespace VendorMgmt.Web.Controllers
{

    public class VendorController : Controller
    {
        public VendorService vs = new VendorService();
        public VendorService Customervs = new VendorService("s");
        // GET: Vendor
        public ActionResult FillInfo(VendorFillInfo model, string LinkGuid = "")
        {
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
        public async Task<ActionResult> GetDetails(string RegistrationCode)
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
            VendorFillInfo model = new VendorFillInfo();
            if (RegistrationCode == string.Empty)
            {
                TempData["Error"] = "No Registration Code Provided";
            }
            else
            {
                model.RegistrationCode = RegistrationCode;
                var VendorMst = vs.VendorMasters.Where(p => p.RegistrationCode == RegistrationCode).FirstOrDefault();
                if (VendorMst == null)
                {
                    TempData["Error"] = "Invalid Registration Code or Registration Code Expired";
                    return PartialView(model);
                }
                model.BusinessName = VendorMst.BusinessName;
                model.VendorEmail = VendorMst.VendorEmail;
                model.DofascoEmail = VendorMst.DofascoEmail;
                model.IsPurchaseManagerApproved = VendorMst.PurchaseManagerApproved;
                model.IsWorldCheckApproved = VendorMst.WorldCheckApproved;
                model.IsTreasuryValidated = VendorMst.TreasuryValidated;
                var Basicinfo = vs.VendorBasicInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.BasicInfo = Basicinfo == null ? new VendorBasicInfo() : Basicinfo;

                var FinancialInfo = vs.VendorFinancialInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.FinancialInfo = FinancialInfo == null ? new VendorFinancialInfo() : FinancialInfo;

                var PrimarySalesInfo = vs.VendorPrimarySalesInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.PrimarySalesInfo = PrimarySalesInfo == null ? new VendorPrimarySalesInfo() : PrimarySalesInfo;

                var SubmittedByInfo = vs.VendorSubmittedByInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.SubmittedByInfo = SubmittedByInfo == null ? new VendorSubmittedByInfo() : SubmittedByInfo;

                var RemittanceInfo = vs.VendorRemittanceInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.RemittanceInfo = RemittanceInfo == null ? new VendorRemittanceInfo() : RemittanceInfo;

                var BankingInfo = vs.VendorBankingInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.BankingInfo = BankingInfo == null ? new VendorBankingInfo() : BankingInfo;

                List<VendorAttachmentInfo> LstAttachment = vs.VendorAttachmentInfos.Where(p => p.VendorId == VendorMst.Id).ToList();
                model.AttachmentInfo = LstAttachment;

                var PurchaseInfo = vs.VendorPurchasingInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.PurchaseInfo = PurchaseInfo == null ? new VendorPurchasingInfo() : PurchaseInfo;

                var WorkFlowInfo = vs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.WorkFlowInfo = WorkFlowInfo == null ? new VendorWorkFlowInfo() : WorkFlowInfo;

                var TreasuryInfo = vs.VendorTreasuryInfos.Where(p => p.VendorId == VendorMst.Id).FirstOrDefault();
                model.TreasuryInfo = TreasuryInfo == null ? new VendorTreasuryInfo() : TreasuryInfo;

                model.VendorId = VendorMst.Id;
                if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
                {
                    model.lstUsers = await MicrosoftGraphClient.GetAllUsers();
                    System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", model.lstUsers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    model.lstUsers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
                }
                ViewData["SpanTreeLevel1"] = new SelectList(vs.SpanTreeLevels.Where(p => p.LevelId == 1).ToList(), "LevelCode", "LevelDescription", model.PurchaseInfo != null ? model.PurchaseInfo.SpendTreelevel1 : "#");
                if (model.PurchaseInfo != null)
                {
                    ViewData["SpanTreeLevel2"] = new SelectList(vs.SpanTreeLevels.Where(p => p.LevelId == 2 && p.ParentLevelText == model.PurchaseInfo.SpendTreelevel1).ToList(), "LevelCode", "LevelDescription", model.PurchaseInfo != null ? model.PurchaseInfo.SpendTreeLevel2 : "");
                    ViewData["SpanTreeLevel3"] = new SelectList(vs.SpanTreeLevels.Where(p => p.LevelId == 3 && p.ParentLevelText == model.PurchaseInfo.SpendTreeLevel2).ToList(), "LevelCode", "LevelDescription", model.PurchaseInfo != null ? model.PurchaseInfo.SpendTreeLevel3 : "");
                    ViewData["SpanTreeLevel4"] = new SelectList(vs.SpanTreeLevels.Where(p => p.LevelId == 4 && p.ParentLevelText == model.PurchaseInfo.SpendTreeLevel3).ToList(), "LevelCode", "LevelDescription", model.PurchaseInfo != null ? model.PurchaseInfo.SpendTreeLevel4 : "");

                }
                model.lstCountries = vs.GetAllCountry();
            }
            return View(model);
        }
        public static string RenderPartialToString(string controlName, object viewData)
        {
            ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

            viewPage.ViewData = new ViewDataDictionary(viewData);
            viewPage.Controls.Add(viewPage.LoadControl(controlName));

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    viewPage.RenderControl(tw);
                }
            }

            return sb.ToString();
        }
        public ActionResult Success()
        {
            return View();
        }
        public async Task<ActionResult> PurchaseApproverConfirmation()
        {
            var action = Functions.Base64Decode(Request.QueryString["Action"]);
            string RegistrationCode = action.Split('|')[0];
            int VendorId = vs.UpdatePurchaseApproval(action.Split('|')[0], action.Split('|')[1] == "Approve" ? true : false);
            var lstUSers = new List<AzureUserList>();
            if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
            {
                lstUSers = await MicrosoftGraphClient.GetAllUsers();
                System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", lstUSers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
            }
            else
            {
                lstUSers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
            }
            var WorkflowDetails = vs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            var PurchaseInfoDetails = vs.VendorPurchasingInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            string Emailbody = "";
            var VendorMst = vs.VendorMasters.Where(p => p.Id == VendorId).FirstOrDefault();
            var RequestorEmail = lstUSers.Where(p => p.DisplayName == WorkflowDetails.RequestorName).FirstOrDefault().EmailAddress;
            var TreasuryInfo = vs.VendorTreasuryInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            EmailTemplateService es = new EmailTemplateService();
            if (action.Split('|')[1] == "Approve")
            {

                //Send Approved Email to Requestor
                Emailbody = es.EmailTemplateByName("Managementapproval").EmailBody;
                Emailbody = Emailbody.Replace("@ApprovedBy", WorkflowDetails.PurchasingManager);
                Emailbody = Emailbody.Replace("@ApprovedComments", WorkflowDetails.PurchaseComments);
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                Functions.SendEmail(RequestorEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);

                var WorldCheckApproverEmail = lstUSers.Where(p => p.DisplayName == WorkflowDetails.WorldCheckApprover).FirstOrDefault().EmailAddress;

                if (PurchaseInfoDetails.TypeofVendorRequest == "New Vendor Activation")
                {
                    //Update Status
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Submitted to Legal");
                    if (WorkflowDetails != null)
                    {
                        Emailbody = es.EmailTemplateByName("WorldCheckApprover").EmailBody;
                        Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName + " Account");
                        Emailbody = Emailbody.Replace("@PurchaseComments", WorkflowDetails.PurchaseComments);
                        Emailbody = Emailbody.Replace("@RequestorName", WorkflowDetails.RequestorName);
                        Emailbody = Emailbody.Replace("@Date", DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt"));
                        Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                        Emailbody = Emailbody.Replace("@ApproveLink", SiteUrl + "/Vendor/WorldCheckApproverConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Approve"));
                        Emailbody = Emailbody.Replace("@DenyLink", SiteUrl + "/Vendor/WorldCheckApproverConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Deny"));
                        Functions.SendEmail(WorldCheckApproverEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);
                    }
                }
                else if (PurchaseInfoDetails.TypeofVendorRequest == "New Vendor Activation - GST# Update")
                {
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Submitted to Legal");
                    Emailbody = es.EmailTemplateByName("WorldCheckApprover").EmailBody;
                    Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName + " Account");
                    Emailbody = Emailbody.Replace("@PurchaseComments", WorkflowDetails.PurchaseComments);
                    Emailbody = Emailbody.Replace("@RequestorName", WorkflowDetails.RequestorName);
                    Emailbody = Emailbody.Replace("@Date", DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt"));
                    Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                    Emailbody = Emailbody.Replace("@ApproveLink", SiteUrl + "/Vendor/WorldCheckApproverConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Approve"));
                    Emailbody = Emailbody.Replace("@DenyLink", SiteUrl + "/Vendor/WorldCheckApproverConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Deny"));
                    Functions.SendEmail(WorldCheckApproverEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);

                }
                else
                {
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Submitted to Treasury");
                    if (TreasuryInfo != null)
                    {
                        var BasicInfo = vs.VendorBasicInfos.Where(p => p.Id == VendorId).FirstOrDefault();
                        var TreasuryApproverEmail = lstUSers.Where(p => p.DisplayName == TreasuryInfo.ActionerName).FirstOrDefault().EmailAddress;
                        //Send Email to ApproverTreasury
                        Emailbody = es.EmailTemplateByName("taxationdetails").EmailBody;
                        Emailbody = Emailbody.Replace("@DisplayName", WorkflowDetails.RequestorName);
                        Emailbody = Emailbody.Replace("@TypeofVendorReq", PurchaseInfoDetails.TypeofVendorRequest);
                        Emailbody = Emailbody.Replace("@VendorName", BasicInfo.VendorName);
                        Emailbody = Emailbody.Replace("@VendorName2", BasicInfo.AlternateName);
                        Emailbody = Emailbody.Replace("@VendorAddress", BasicInfo.BusinessAddress);
                        Emailbody = Emailbody.Replace("@VendorCity", BasicInfo.BusinessCity);
                        Emailbody = Emailbody.Replace("@VendorState", BasicInfo.BusinessState);
                        Emailbody = Emailbody.Replace("@VendorPostalCode", BasicInfo.BusinessZip);
                        Emailbody = Emailbody.Replace("@VendorCountry", BasicInfo.Country);
                        Emailbody = Emailbody.Replace("@RemiInfoCurrency", BasicInfo.PaymentCurrency);
                        Emailbody = Emailbody.Replace("@VendorGST_Applicable", BasicInfo.GSTApplicable == true ? "Yes" : "No");
                        Emailbody = Emailbody.Replace("@VendorGSTRegistration_No", BasicInfo.GSTNumber);
                        Emailbody = Emailbody.Replace("@VendorPFDEmail", VendorMst.VendorEmail);
                        Emailbody = Emailbody.Replace("@VendorPFDPhone_Number", BasicInfo.Phonenumber);
                        Functions.SendEmail(TreasuryApproverEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);
                    }
                }

            }
            else
            {
                vs.UpdateStatus(VendorMst.RegistrationCode, "Rejected by Purchasing Manager");
                //Send notification to Requestor in case of Rejection by Purchasing Manager
                Emailbody = es.EmailTemplateByName("PurchaseRejection").EmailBody;
                Emailbody = Emailbody.Replace("@RejectedBy", WorkflowDetails.PurchasingManager);
                Emailbody = Emailbody.Replace("@Comments", WorkflowDetails.PurchaseComments);
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                Functions.SendEmail(RequestorEmail, es.EmailTemplateByName("PurchaseRejection").EmailSubject, Emailbody, false);


            }
            return Content("Sucess");
        }
        public async Task<ActionResult> WorldCheckApproverConfirmation()
        {
            var action = Functions.Base64Decode(Request.QueryString["Action"]);
            string RegistrationCode = action.Split('|')[0];
            int VendorId = vs.UpdateWorldCheckApproval(action.Split('|')[0], action.Split('|')[1] == "Approve" ? true : false);
            var lstUSers = new List<AzureUserList>();
            if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
            {
                lstUSers = await MicrosoftGraphClient.GetAllUsers();
                System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", lstUSers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
            }
            else
            {
                lstUSers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
            }
            var WorkflowDetails = vs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            string Emailbody = "";
            EmailTemplateService es = new EmailTemplateService();
            var RequestorEmail = lstUSers.Where(p => p.DisplayName == WorkflowDetails.RequestorName).FirstOrDefault().EmailAddress;
            var VendorMst = vs.VendorMasters.Where(p => p.Id == VendorId).FirstOrDefault();
            var PurchaseInfoDetails = vs.VendorPurchasingInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            var BasicInfo = vs.VendorBasicInfos.Where(p => p.Id == VendorId).FirstOrDefault();
            if (action.Split('|')[1] == "Approve")
            {
                vs.UpdateStatus(RegistrationCode, "Submitted to Treasury");
                //Send Legal Approved Email to Requestor
                Emailbody = es.EmailTemplateByName("Managementapproval").EmailBody;
                Emailbody = Emailbody.Replace("@ApprovedBy", WorkflowDetails.PurchasingManager);
                Emailbody = Emailbody.Replace("@ApprovedComments", WorkflowDetails.PurchaseComments);
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                Functions.SendEmail(RequestorEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);
                //Send Email to Tax Department
                Emailbody = es.EmailTemplateByName("taxationdetails").EmailBody;
                Emailbody = Emailbody.Replace("@DisplayName", WorkflowDetails.RequestorName);
                Emailbody = Emailbody.Replace("@TypeofVendorReq", PurchaseInfoDetails.TypeofVendorRequest);
                Emailbody = Emailbody.Replace("@VendorName", BasicInfo.VendorName);
                Emailbody = Emailbody.Replace("@VendorName2", BasicInfo.AlternateName);
                Emailbody = Emailbody.Replace("@VendorAddress", BasicInfo.BusinessAddress);
                Emailbody = Emailbody.Replace("@VendorCity", BasicInfo.BusinessCity);
                Emailbody = Emailbody.Replace("@VendorState", BasicInfo.BusinessState);
                Emailbody = Emailbody.Replace("@VendorPostalCode", BasicInfo.BusinessZip);
                Emailbody = Emailbody.Replace("@VendorCountry", BasicInfo.Country);
                Emailbody = Emailbody.Replace("@RemiInfoCurrency", BasicInfo.PaymentCurrency);
                Emailbody = Emailbody.Replace("@VendorGST_Applicable", BasicInfo.GSTApplicable == true ? "Yes" : "No");
                Emailbody = Emailbody.Replace("@VendorGSTRegistration_No", BasicInfo.GSTNumber);
                Emailbody = Emailbody.Replace("@VendorPFDEmail", VendorMst.VendorEmail);
                Emailbody = Emailbody.Replace("@VendorPFDPhone_Number", BasicInfo.Phonenumber);
                Functions.SendEmail("heather.mcclure@arcelormittal.com;olga.chernobab@arcelormittal.com", "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);

                if (PurchaseInfoDetails.TypeofVendorRequest == "New Vendor Activation")
                {
                }
                else if (PurchaseInfoDetails.TypeofVendorRequest == "New Vendor Activation - GST# Update")
                {
                }
                else
                {

                }
            }
            else
            {

                vs.UpdateStatus(RegistrationCode, "Rejected by Legal");
                //Send notification to Requestor in case of Rejection 
                Emailbody = es.EmailTemplateByName("WorldCheckRejection").EmailBody;
                Emailbody = Emailbody.Replace("@RejectedBy", WorkflowDetails.WorldCheckApprover + ". This Vendor creation process will end.");
                Emailbody = Emailbody.Replace("@Comments", WorkflowDetails.PurchaseComments);
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                Functions.SendEmail(RequestorEmail, es.EmailTemplateByName("WorldCheckRejection").EmailSubject, Emailbody, false);
                vs.AssignedBackToRequestor(RegistrationCode);
            }
            return Content("Sucess");
        }

        public async Task<ActionResult> TreasuryActionerConfirmation()
        {
            var action = Functions.Base64Decode(Request.QueryString["Action"]);
            string RegistrationCode = action.Split('|')[0];
            int VendorId = vs.UpdateWorldCheckApproval(action.Split('|')[0], action.Split('|')[1] == "Approve" ? true : false);
            var VendorMst = vs.VendorMasters.Where(p => p.Id == VendorId).FirstOrDefault();
            var PurchaseInfoDetails = vs.VendorPurchasingInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            var lstUSers = new List<AzureUserList>();
            if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
            {
                lstUSers = await MicrosoftGraphClient.GetAllUsers();
                System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", lstUSers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
            }
            else
            {
                lstUSers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
            }
            var WorkflowDetails = vs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            var TreasuryInfo = vs.VendorTreasuryInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
            string Emailbody = "";
            EmailTemplateService es = new EmailTemplateService();
            var RequestorEmail = lstUSers.Where(p => p.DisplayName == WorkflowDetails.RequestorName).FirstOrDefault().EmailAddress;

            var TreasuryActionerEmail = lstUSers.Where(p => p.DisplayName == TreasuryInfo.ActionerName).FirstOrDefault().EmailAddress;
            var TreasuryActioner2Email = lstUSers.Where(p => p.DisplayName == TreasuryInfo.Level2ApproverName).FirstOrDefault().EmailAddress;



            if (action.Split('|')[1] == "Approve")
            {
                vs.UpdateStatus(RegistrationCode, "Submitted to 2nd Level Approver");
                //Send notification email to TreasuryActioner
                Emailbody = es.EmailTemplateByName("NotificationTreasuryActioner").EmailBody;
                Functions.SendEmail(TreasuryActionerEmail, es.EmailTemplateByName("NotificationTreasuryActioner").EmailSubject, Emailbody, false);
                //Email Treasury Approver 2 with Vendor Details
                Emailbody = es.EmailTemplateByName("secondlevelapprovaltreasury").EmailBody;
                Emailbody = Emailbody.Replace("@RequestorName", WorkflowDetails.RequestorName);
                Emailbody = Emailbody.Replace("@TypeofVendorReq", PurchaseInfoDetails.TypeofVendorRequest);
                Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName);
                Emailbody = Emailbody.Replace("@TreasuryActioner", TreasuryInfo.ActionerName);
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);

                Functions.SendEmail(TreasuryActioner2Email, es.EmailTemplateByName("secondlevelapprovaltreasury").EmailSubject, Emailbody, false);

            }
            else
            {
                vs.UpdateStatus(RegistrationCode, "Rejected by Treasury");
                //Treasury Rejection Email to Requestor
                Emailbody = es.EmailTemplateByName("TreasuryRejectionToRequestor").EmailBody;
                Functions.SendEmail(RequestorEmail, es.EmailTemplateByName("TreasuryRejectionToRequestor").EmailSubject, Emailbody, false);


            }
            return Content("Sucess");
        }
        #region Customer Fill Entry form Save requests
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
                model.BasicInfo.Country = Request["BasicInfo_Country"];
                //TryUpdateModel<VendorBasicInfo>(model.BasicInfo, new string[] { "VendorName", "AlternateName", "Name", "Signee_Name", "Signee_Phone", "Signee_Email", "OperationsContact", "OperationsPhone", "OperationsEmail", "GlobalAddressBook", "DisplayUserHubProfile", "AccountGroup" });
                //vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                Customervs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                Customervs.VendorFinancialInfo_InsertOrUpdate(model.FinancialInfo);
                return Json(new { BasicInfoId = model.BasicInfo.Id, FinancialInfoId = model.FinancialInfo.Id }, JsonRequestBehavior.AllowGet);
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
                model.PrimarySalesInfo.VendorId = model.VendorId;
                Customervs.VendorPrimarySalesInfo_InsertOrUpdate(model.PrimarySalesInfo);
                model.SubmittedByInfo.VendorId = model.VendorId;
                Customervs.VendorSubmittedByInfo_InsertOrUpdate(model.SubmittedByInfo);
                model.RemittanceInfo.VendorId = model.VendorId;
                model.RemittanceInfo.Country = Request["RemittanceInfo_Country"];
                Customervs.VendorRemittanceInfo_InsertOrUpdate(model.RemittanceInfo);
                return Json(new { PrimarySalesInfoId = model.PrimarySalesInfo.Id, SubmittedByInfoId = model.SubmittedByInfo.Id, RemittanceInfoId = model.RemittanceInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PrimarySalesInfoId = 0, SubmittedByInfoId = 0, RemittanceInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab3(VendorFillInfo model)
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
                Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + model.RegistrationCode);
                Functions.SendEmail(model.DofascoEmail, "Confirmation", Emailbody, false);
                //update Application Status
                vs.UpdateStatus(model.RegistrationCode, "Submitted by Vendor");

                //Transfer Data from Customer to Admin Portal DB
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => new Worker().StartProcessing(model.VendorId, cancellationToken));

                return Json(new { BankingInfoId = model.BankingInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { BankingInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveTab4(VendorFillInfo model)
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
                model.PurchaseInfo.VendorId = model.VendorId;
                Customervs.VendorPurchasingInfo_InsertOrUpdate(model.PurchaseInfo);
                model.WorkFlowInfo.VendorId = model.VendorId;
                Customervs.VendorWorkFlowInfo_InsertOrUpdate(model.WorkFlowInfo);

                return Json(new { PurchaseInfoId = model.PurchaseInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PurchaseInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab5(VendorFillInfo model)
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
                model.TreasuryInfo.VendorId = model.VendorId;
                Customervs.VendorTreasuryInfo_InsertOrUpdate(model.TreasuryInfo);
                return Json(new { TreasuryInfoId = model.TreasuryInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { TreasuryInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestFileUpload()
        {
            List<VendorAttachmentInfo> lst = vs.VendorAttachmentInfos.Where(p => p.VendorId == 1).ToList();
            ViewBag.JavaScriptFunction = "successToast();";
            return View(lst);
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
                fname = Path.Combine(Server.MapPath("~/Attachments/"), t.Id.ToString() + "_" + t.FileName);
                file.SaveAs(fname);
            }
            return Json("", JsonRequestBehavior.AllowGet);
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
        public List<SelectListItem> PopulateDropdownListValues(List<string> lst, string SelectedValue, bool AddDefaultValue = true)
        {

            List<SelectListItem> result = (from p in lst.AsEnumerable()
                                           select new SelectListItem
                                           {
                                               Text = p.Trim(),
                                               Value = p.Trim()
                                           }).ToList();

            if (AddDefaultValue)
            {
                string SelectText = "-- SELECT --";
                result.Insert(0, new SelectListItem
                {
                    Text = SelectText,
                    Value = SelectText
                });
            }
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                result.Find(c => c.Value == SelectedValue).Selected = true;
            }
            return result;
        }
        #endregion

        #region Admin Save Events

        [HttpPost]
        public JsonResult SaveTab1Admin(VendorFillInfo model)
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
                //vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);\
                model.BasicInfo.Country = Request["BasicInfo_Country"];
                vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                vs.VendorFinancialInfo_InsertOrUpdate(model.FinancialInfo);
                model.PrimarySalesInfo.VendorId = model.VendorId;
                vs.VendorPrimarySalesInfo_InsertOrUpdate(model.PrimarySalesInfo);
                model.SubmittedByInfo.VendorId = model.VendorId;
                vs.VendorSubmittedByInfo_InsertOrUpdate(model.SubmittedByInfo);

                return Json(new { PrimarySalesInfoId = model.PrimarySalesInfo.Id, SubmittedByInfoId = model.SubmittedByInfo.Id, BasicInfoId = model.BasicInfo.Id, FinancialInfoId = model.FinancialInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { id = 0, PrimarySalesInfoId = 0, SubmittedByInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab2Admin(VendorFillInfo model)
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


                return Json(new { PrimarySalesInfoId = model.PrimarySalesInfo.Id, SubmittedByInfoId = model.SubmittedByInfo.Id, RemittanceInfoId = model.RemittanceInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PrimarySalesInfoId = 0, SubmittedByInfoId = 0, RemittanceInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab3Admin(VendorFillInfo model)
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
                model.BankingInfo.VendorId = model.VendorId;

                model.BankingInfo.Country = Request["BankingInfo_Country"];
                vs.VendorBankingInfo_InsertOrUpdate(model.BankingInfo);
                model.RemittanceInfo.VendorId = model.VendorId;
                model.RemittanceInfo.Country = Request["RemittanceInfo_Country"];
                vs.VendorRemittanceInfo_InsertOrUpdate(model.RemittanceInfo);
                return Json(new { RemittanceInfoId = model.RemittanceInfo.Id, BankingInfoId = model.BankingInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { BankingInfoId = 0, RemittanceInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTab4Admin(VendorFillInfo model)
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
                model.PurchaseInfo.VendorId = model.VendorId;
                vs.VendorPurchasingInfo_InsertOrUpdate(model.PurchaseInfo);
                model.WorkFlowInfo.VendorId = model.VendorId;
                //model.WorkFlowInfo.PurchasingManager = Request.Form["WorkFlowInfo_PurchasingManager"];
                //model.WorkFlowInfo.RequestorName = Request.Form["WorkFlowInfo_RequestorName"];
                //model.WorkFlowInfo.WorldCheckApprover = Request.Form["WorkFlowInfo_WorldCheckApprover"];
                vs.VendorWorkFlowInfo_InsertOrUpdate(model.WorkFlowInfo);
                if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
                {
                    model.lstUsers = await MicrosoftGraphClient.GetAllUsers();
                    System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", model.lstUsers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    model.lstUsers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
                    model.PurchaseApproverEmail = model.lstUsers.Where(p => p.DisplayName == model.WorkFlowInfo.PurchasingManager).FirstOrDefault().EmailAddress;
                }
                if (VendorMst.PurchaseManagerApproved == false && VendorMst.WorldCheckApproved == false)
                {
                    string Emailbody = "";
                    EmailTemplateService es = new EmailTemplateService();
                    Emailbody = es.EmailTemplateByName("PurchaseApproval").EmailBody;
                    Emailbody = Emailbody.Replace("@VendorName", model.BusinessName);
                    Emailbody = Emailbody.Replace("@PurchaseComments", model.WorkFlowInfo.PurchaseComments);
                    Emailbody = Emailbody.Replace("@RequestorName", model.WorkFlowInfo.RequestorName);
                    Emailbody = Emailbody.Replace("@ApproveLink", SiteUrl + "/Vendor/PurchaseApproverConfirmation?Action=" + Functions.Base64Encode(model.RegistrationCode + "|Approve"));
                    Emailbody = Emailbody.Replace("@DenyLink", SiteUrl + "/Vendor/PurchaseApproverConfirmation?Action=" + Functions.Base64Encode(model.RegistrationCode + "|Deny"));
                    Functions.SendEmail(string.IsNullOrEmpty(model.PurchaseApproverEmail) ? "hardikce.08@gmail.com" : model.PurchaseApproverEmail, "Approval", Emailbody, false);
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Submitted to Purch Manager");
                }
                return Json(new { PurchaseInfoId = model.PurchaseInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PurchaseInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> SaveTab5Admin(VendorFillInfo model)
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
                model.TreasuryInfo.VendorId = model.VendorId;
                model.TreasuryInfo.ActionerName = Request.Form["TreasuryInfo_ActionerName"];
                model.TreasuryInfo.ChecklistInfo1 = Convert.ToBoolean(Request.Form["TreasuryInfo_ChecklistInfo1"]);
                model.TreasuryInfo.ChecklistInfo2 = Convert.ToBoolean(Request.Form["TreasuryInfo_ChecklistInfo2"]);
                model.TreasuryInfo.Level2ApproverName = Request.Form["TreasuryInfo_Level2ApproverName"];
                model.TreasuryInfo.Level2ChecklistInfo = Convert.ToBoolean(Request.Form["TreasuryInfo_Level2ChecklistInfo"]);
                EmailTemplateService es = new EmailTemplateService();
                var lstUSers = new List<AzureUserList>();
                if (System.Web.HttpContext.Current.Cache["lstAzureUsers"] == null)
                {
                    lstUSers = await MicrosoftGraphClient.GetAllUsers();
                    System.Web.HttpContext.Current.Cache.Add("lstAzureUsers", lstUSers, null, DateTime.Now.AddMinutes(120), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                }
                else
                {
                    lstUSers = System.Web.HttpContext.Current.Cache["lstAzureUsers"] as List<AzureUserList>;
                }
                var PurchaseInfoDetails = vs.VendorPurchasingInfos.Where(p => p.VendorId == model.VendorId).FirstOrDefault();
                var WorkflowDetails = vs.VendorWorkFlowInfos.Where(p => p.VendorId == model.VendorId).FirstOrDefault();
                var RequestorEmail = lstUSers.Where(p => p.DisplayName == WorkflowDetails.RequestorName).FirstOrDefault().EmailAddress;
                var TreasuryActionerEmail = lstUSers.Where(p => p.DisplayName == model.TreasuryInfo.ActionerName).FirstOrDefault().EmailAddress;
                var TreasuryActioner2Email = lstUSers.Where(p => p.DisplayName == model.TreasuryInfo.Level2ApproverName).FirstOrDefault().EmailAddress;

                vs.VendorTreasuryInfo_InsertOrUpdate(model.TreasuryInfo);
                if (model.IsTreasuryValidated == false)
                {
                    //Update status to Validation Pending
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Validation Pending");
                    vs.UpdateTreasuryvlidated(VendorMst.RegistrationCode, true);
                    // send Pending Validation Email to Requestor
                    var Emailbody = es.EmailTemplateByName("pendingvalidationnotification").EmailBody;
                    Emailbody = Emailbody.Replace("@TreasuryActioner", model.TreasuryInfo.ActionerName);
                    Emailbody = Emailbody.Replace("@TypeofVendorReq", PurchaseInfoDetails.TypeofVendorRequest);
                    Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName);
                    Emailbody = Emailbody.Replace("@VendorNo", model.TreasuryInfo.VendorNumber);
                    Functions.SendEmail(RequestorEmail, es.EmailTemplateByName("pendingvalidationnotification").EmailSubject, Emailbody, false);
                    // send email to Treasury Actioner for Pending notification
                    Emailbody = es.EmailTemplateByName("PendingTreasuryActionerNotification").EmailBody;
                    Functions.SendEmail(TreasuryActionerEmail, es.EmailTemplateByName("PendingTreasuryActionerNotification").EmailSubject, Emailbody, false);


                    //send email to Treasury Approval
                    Emailbody = es.EmailTemplateByName("WorldCheckApprover").EmailBody;
                    Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName + " Account");
                    Emailbody = Emailbody.Replace("@PurchaseComments", WorkflowDetails.PurchaseComments);
                    Emailbody = Emailbody.Replace("@RequestorName", WorkflowDetails.RequestorName);
                    Emailbody = Emailbody.Replace("@Date", DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt"));
                    Emailbody = Emailbody.Replace("{ShortUrl}", SiteUrl + "/Vendor/GetDetails?RegistrationCode=" + VendorMst.RegistrationCode);
                    Emailbody = Emailbody.Replace("@ApproveLink", SiteUrl + "/Vendor/TreasuryActionerConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Approve"));
                    Emailbody = Emailbody.Replace("@DenyLink", SiteUrl + "/Vendor/TreasuryActionerConfirmation?Action=" + Functions.Base64Encode(VendorMst.RegistrationCode + "|Deny"));
                    Functions.SendEmail(TreasuryActionerEmail, "New Vendor Actiation - " + VendorMst.BusinessName + " Account", Emailbody, false);
                }
                if (model.IsTreasuryValidated == true)
                {
                    //Update status to Process Complete
                    vs.UpdateStatus(VendorMst.RegistrationCode, "Process Completed");
                    var Emailbody = es.EmailTemplateByName("pendingvalidationnotification").EmailBody;
                    Emailbody = Emailbody.Replace("@TreasuryActioner", model.TreasuryInfo.ActionerName);
                    Emailbody = Emailbody.Replace("@TypeofVendorReq", PurchaseInfoDetails.TypeofVendorRequest);
                    Emailbody = Emailbody.Replace("@VendorName", VendorMst.BusinessName);
                    Emailbody = Emailbody.Replace("@VendorNo", model.TreasuryInfo.VendorNumber);
                    Functions.SendEmail(RequestorEmail, "Vendor Activation Process Complete", Emailbody, false);
                    Functions.SendEmail(TreasuryActionerEmail, "Vendor Activation Process Complete", Emailbody, false);
                    Functions.SendEmail(TreasuryActioner2Email, "Vendor Activation Process Complete", Emailbody, false);

                }
                return Json(new { TreasuryInfoId = model.TreasuryInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { TreasuryInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFilesAdmin()
        {
            string VendorId = Request["VendorId"];
            //Save Files if its Exist 
            HttpFileCollectionBase files = Request.Files;
            string fids = "";
            for (int i = 0; i < files.Count; i++)
            {
                int VendorID = Convert.ToInt32(VendorId);
                //// Remove Existing files 
                //List<VendorAttachmentInfo> LstAttachment = vs.VendorAttachmentInfos.Where(p => p.VendorId == VendorID).ToList();
                //foreach (var item in LstAttachment)
                //{
                //    var obj = vs.VendorAttachmentInfos.Where(p => p.Id == item.Id).FirstOrDefault();
                //    string FilePath = Path.Combine(Server.MapPath("~/Attachments/"), VendorID.ToString() + "_" + obj.FileName);
                //    if (System.IO.File.Exists(FilePath))
                //    {
                //        System.IO.File.Delete(FilePath);
                //    }
                //    Customervs.VendorAttachmentInfo_Remove(obj);
                //}


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
                vs.VendorAttachmentInfo_InsertOrUpdate(t);
                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Attachments/"), t.Id.ToString() + "_" + t.FileName);
                file.SaveAs(fname);
                fids += t.Id + ",";
            }
            return Json(fids, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveFilesAdmin(string Id)
        {
            int ID = Convert.ToInt32(Id);
            var obj = vs.VendorAttachmentInfos.Where(p => p.Id == ID).FirstOrDefault();
            vs.VendorAttachmentInfo_Remove(obj);
            return Content("test");
        }
        #endregion

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