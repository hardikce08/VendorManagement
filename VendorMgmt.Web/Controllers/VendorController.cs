﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorMgmt.DataAccess;
using VendorMgmt.DataAccess.Model;

namespace VendorMgmt.Web.Controllers
{

    public class VendorController : Controller
    {
        public VendorService vs = new VendorService();
        // GET: Vendor
        public ActionResult FillInfo(VendorFillInfo model, string RegistrationCode = "")
        {
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
                    TempData["Error"] = "Invalid Registration Code";
                    return View(model);
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
                 
                model.VendorId = VendorMst.Id;
            }
            return View(model);
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
                vs.VendorBasicInfo_InsertOrUpdate(model.BasicInfo);
                vs.VendorFinancialInfo_InsertOrUpdate(model.FinancialInfo);
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
                vs.VendorBankingInfo_InsertOrUpdate(model.BankingInfo);

                

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
                vs.VendorPurchasingInfo_InsertOrUpdate(model.PurchaseInfo);
                
                return Json(new { PurchaseInfoId = model.PurchaseInfo.Id   }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { PurchaseInfoId = 0  }, JsonRequestBehavior.AllowGet);
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
                //// Remove Existing files 
                //List<VendorAttachmentInfo> LstAttachment = vs.VendorAttachmentInfos.Where(p => p.VendorId == VendorMst.Id).ToList();
                //int ID = Convert.ToInt32(Id);
                //var obj = vs.VendorAttachmentInfos.Where(p => p.Id == ID).FirstOrDefault();
                //vs.VendorAttachmentInfo_Remove(obj);

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
                VendorService vs = new VendorService();
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
        public ActionResult RemoveFiles(string Id)
        {
            int ID = Convert.ToInt32(Id);
            var obj = vs.VendorAttachmentInfos.Where(p => p.Id == ID).FirstOrDefault();
            vs.VendorAttachmentInfo_Remove(obj);
            return Content("test");
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
    }
}