using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using VendorMgmt.DataAccess;
using VendorMgmt.DataAccess.Model;

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

            if (model.UserRegistrationCode == VendorMst.RegistrationCode && model.Email.ToLower() == VendorMst.VendorEmail.ToLower() && model.CompanyName.ToLower()==VendorMst.BusinessName.ToLower())
            {
                return Json(new { data = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "Authentication Failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDetails(string RegistrationCode)
        {
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

                ViewData["SpanTreeLevel1"] = new SelectList(vs.SpanTreeLevels.Where(p => p.LevelId == 1).ToList(), "LevelCode", "LevelDescription");

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
                Customervs.VendorBankingInfo_InsertOrUpdate(model.BankingInfo);
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
                //Transfer Data from Customer to Admin Portal DB
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => new Worker().StartProcessing(model.VendorId, cancellationToken));
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
                //vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                vs.VendorFinancialInfo_InsertOrUpdate(model.FinancialInfo);
                return Json(new { BasicInfoId = model.BasicInfo.Id, FinancialInfoId = model.FinancialInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
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
                model.PrimarySalesInfo.VendorId = model.VendorId;
                vs.VendorPrimarySalesInfo_InsertOrUpdate(model.PrimarySalesInfo);
                model.SubmittedByInfo.VendorId = model.VendorId;
                vs.VendorSubmittedByInfo_InsertOrUpdate(model.SubmittedByInfo);
                model.RemittanceInfo.VendorId = model.VendorId;
                vs.VendorRemittanceInfo_InsertOrUpdate(model.RemittanceInfo);
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
                vs.VendorBankingInfo_InsertOrUpdate(model.BankingInfo);
                return Json(new { BankingInfoId = model.BankingInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { BankingInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveTab4Admin(VendorFillInfo model)
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
                vs.VendorWorkFlowInfo_InsertOrUpdate(model.WorkFlowInfo);
                return Json(new { PurchaseInfoId = model.PurchaseInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PurchaseInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveTab5Admin(VendorFillInfo model)
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
                vs.VendorTreasuryInfo_InsertOrUpdate(model.TreasuryInfo);
                return Json(new { TreasuryInfoId = model.TreasuryInfo.Id }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { TreasuryInfoId = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFilesAdmin()
        {
            string VendorId = Request["VendorId"];
            //Save Files if its Exist 
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                int VendorID = Convert.ToInt32(VendorId);
                // Remove Existing files 
                List<VendorAttachmentInfo> LstAttachment = vs.VendorAttachmentInfos.Where(p => p.VendorId == VendorID).ToList();
                foreach (var item in LstAttachment)
                {
                    var obj = vs.VendorAttachmentInfos.Where(p => p.Id == item.Id).FirstOrDefault();
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
                vs.VendorAttachmentInfo_InsertOrUpdate(t);
                // Get the complete folder path and store the file inside it.      
                fname = Path.Combine(Server.MapPath("~/Attachments/"), t.Id.ToString() + "_" + t.FileName);
                file.SaveAs(fname);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveFilesAdmin(string Id)
        {
            int ID = Convert.ToInt32(Id);
            var obj = vs.VendorAttachmentInfos.Where(p => p.Id == ID).FirstOrDefault();
            vs.VendorAttachmentInfo_Remove(obj);
            return Content("test");
        }
        #endregion
    }
}