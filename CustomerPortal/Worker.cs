using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using VendorMgmt.DataAccess;

namespace CustomerPortal
{
    public class Worker
    {
        public VendorService vs = new VendorService();
        public VendorService Customervs = new VendorService("s");
        public void StartProcessing(int VendorId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {

                var Basicinfo = Customervs.VendorBasicInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                Basicinfo.Id = 0;
                vs.VendorBasicInfo_InsertOrUpdate(Basicinfo);
                var FinancialInfo = Customervs.VendorFinancialInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                FinancialInfo.Id = 0;
                vs.VendorFinancialInfo_InsertOrUpdate(FinancialInfo);
                var PrimarySalesInfo = Customervs.VendorPrimarySalesInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                PrimarySalesInfo.Id = 0;
                vs.VendorPrimarySalesInfo_InsertOrUpdate(PrimarySalesInfo);
                var SubmittedByInfo = Customervs.VendorSubmittedByInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                SubmittedByInfo.Id = 0;
                vs.VendorSubmittedByInfo_InsertOrUpdate(SubmittedByInfo);
                var RemittanceInfo = Customervs.VendorRemittanceInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                RemittanceInfo.Id = 0;
                vs.VendorRemittanceInfo_InsertOrUpdate(RemittanceInfo);
                var BankingInfo = Customervs.VendorBankingInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                BankingInfo.Id = 0;
                vs.VendorBankingInfo_InsertOrUpdate(BankingInfo);
                var LstAttachment = Customervs.VendorAttachmentInfos.Where(p => p.VendorId == VendorId).ToList();
                foreach (var attachment in LstAttachment)
                {
                    attachment.Id = 0;
                    vs.VendorAttachmentInfo_InsertOrUpdate(attachment);
                }
                var PurchaseInfo = Customervs.VendorPurchasingInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                if (PurchaseInfo != null)
                {
                    PurchaseInfo.Id = 0;
                    vs.VendorPurchasingInfo_InsertOrUpdate(PurchaseInfo);
                }
                var WorkFlowInfo = Customervs.VendorWorkFlowInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                if (WorkFlowInfo != null)
                {
                    WorkFlowInfo.Id = 0;
                    vs.VendorWorkFlowInfo_InsertOrUpdate(WorkFlowInfo);
                }
                var TreasuryInfo = Customervs.VendorTreasuryInfos.Where(p => p.VendorId == VendorId).FirstOrDefault();
                if (TreasuryInfo != null)
                {
                    TreasuryInfo.Id = 0;
                    vs.VendorTreasuryInfo_InsertOrUpdate(TreasuryInfo);
                }
                vs.DeleteVendorFromCustomer(VendorId);
                ProcessCancellation();
            }
            catch (Exception ex)
            {
                ProcessCancellation();
            }
        }
        private void ProcessCancellation()
        {
            Thread.Sleep(1000);
        }
    }
}