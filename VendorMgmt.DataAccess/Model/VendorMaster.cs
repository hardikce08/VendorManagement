using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorMgmt.DataAccess.Model
{
    public class VendorMaster
    {

        public int Id { get; set; }

        public string BusinessName { get; set; }

        public string RegistrationCode { get; set; }

        public string VendorEmail { get; set; }

        public string DofascoEmail { get; set; }

        public bool nsKnox { get; set; }

        public bool EmailSent { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class VendorFillInfo
    {
        public VendorBasicInfo BasicInfo { get; set; }
        public VendorFinancialInfo FinancialInfo { get; set; }
        public VendorPrimarySalesInfo PrimarySalesInfo { get; set; }

        public VendorSubmittedByInfo SubmittedByInfo { get; set; }
        public VendorRemittanceInfo RemittanceInfo { get; set; }
        public VendorBankingInfo BankingInfo { get; set; }

        public List<VendorAttachmentInfo> AttachmentInfo { get; set; }
        public VendorPurchasingInfo PurchaseInfo { get; set; }
        public VendorWorkFlowInfo WorkFlowInfo { get; set; }

        public VendorTreasuryInfo TreasuryInfo { get; set; }
        public string RegistrationCode { get; set; }
        public int VendorId { get; set; }
    }

    public class VendorBasicInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string VendorName { get; set; }

        public string AlternateName { get; set; }

        public string SCAC { get; set; }

        public string BusinessAddress { get; set; }

        public string BusinessCity { get; set; }

        public string BusinessState { get; set; }

        public string BusinessZip { get; set; }

        public string Country { get; set; }

        public string Phonenumber { get; set; }

        public string Website { get; set; }

        public string PaymentCurrency { get; set; }

        public bool GSTApplicable { get; set; }

        public string GSTNumber { get; set; }

        public string DiversityBusinessCertification { get; set; }

        public bool IsCouncilMember { get; set; }

        public string CouncilName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorFinancialInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class VendorPrimarySalesInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorSubmittedByInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string SubmittedByName { get; set; }

        public string Email { get; set; }

        public string DofascoEmail { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorRemittanceInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string MailingAddress { get; set; }

        public string MailingCity { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string SpecialPayment { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorBankingInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string InstitutionNo { get; set; }

        public string TransitNo { get; set; }

        public string BankAccountNo { get; set; }

        public string ABA_RoutingNumber { get; set; }

        public string Swift_BICCode { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class VendorAttachmentInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorPurchasingInfo
    {
        public int Id { get; set; }

        public int VendorId { get; set; }

        public bool TradingPartner { get; set; }

        public string AccountGroupName { get; set; }

        public string TypeofVendorRequest { get; set; }

        public bool ConflictOfInterest { get; set; }

        public string PaymentTerms { get; set; }

        public string SpendTreelevel1 { get; set; }

        public string SpendTreeLevel2 { get; set; }

        public string SpendTreeLevel3 { get; set; }

        public string SpendTreeLevel4 { get; set; }
        public string SAPBusType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorWorkFlowInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string RequestorName { get; set; }

        public string PurchasingManager { get; set; }

        public string VendorNumber { get; set; }

        public string PurchaseComments { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class VendorTreasuryInfo
    {

        public int Id { get; set; }

        public int VendorId { get; set; }

        public string ActionerName { get; set; }

        public string VendorNumber { get; set; }

        public bool Validated { get; set; }

        public bool ChecklistInfo1 { get; set; }

        public bool ChecklistInfo2 { get; set; }

        public string Level2ApproverName { get; set; }

        public bool Level2ChecklistInfo { get; set; }

        public string Level2Comments { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    #region Vendor ListView
    public class VendorListView
    {
        public string VendorName { get; set; }
        public string RequestorName { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
        #endregion
    }
