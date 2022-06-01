using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorMgmt.DataAccess.Model;
using EF = VendorMgmt.Model;
namespace VendorMgmt.DataAccess
{
    public class VendorService : ConnectionHelper
    {
        EF.VendorEntities db = null;
        public VendorService()
        {
            db = new EF.VendorEntities(EntityConnectionString);
        }
        public VendorService(string s)
        {
            db = new EF.VendorEntities(CustomerEntityConnectionString);
        }
        public VendorService(ObjectContext context)
        {
            db = context as EF.VendorEntities;
        }
        public ObjectContext DbContext
        {
            get
            {
                return db as ObjectContext;
            }
        }

        public IQueryable<VendorMaster> VendorMasters
        {
            get
            {
                return from v in db.VendorMaster
                       select new VendorMaster
                       {
                           Id = v.Id,
                           BusinessName = v.BusinessName,
                           RegistrationCode = v.RegistrationCode,
                           VendorEmail = v.VendorEmail,
                           DofascoEmail = v.DofascoEmail,
                           nsKnox = v.nsKnox,
                           EmailSent = v.EmailSent,
                           CreatedDate = v.CreatedDate,
                           LinkExpired = v.LinkExpired,
                           LinkGuid = v.LinkGuid,
                           PurchaseManagerApproved = v.PurchaseManagerApproved,
                           WorldCheckApproved = v.WorldCheckApproved,
                           TreasuryValidated = v.TreasuryValidated
                       };
            }
        }
        public IQueryable<SpanTreeLevel> SpanTreeLevels
        {
            get
            {
                return from s in db.SpanTreeLevel
                       select new SpanTreeLevel
                       {
                           Id = s.Id,
                           LevelCode = s.LevelCode,
                           LevelDescription = s.LevelDescription,
                           LevelId = s.LevelId,
                           ParentLevelText = s.ParentLevelText,
                       };
            }
        }
        public void VendorMaster_InsertOrUpdate(VendorMaster v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorMaster
                {
                    BusinessName = v.BusinessName,
                    RegistrationCode = v.RegistrationCode,
                    VendorEmail = v.VendorEmail,
                    DofascoEmail = v.DofascoEmail,
                    nsKnox = v.nsKnox,
                    EmailSent = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    LinkExpired = false,
                    LinkGuid = v.LinkGuid,
                    Status = "Validation Pending",
                    PurchaseManagerApproved = false,
                    WorldCheckApproved = false,
                    TreasuryValidated = false
                };

                db.VendorMaster.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorMaster.Where(p => p.Id == v.Id).Single();
                u.BusinessName = v.BusinessName;
                u.RegistrationCode = v.RegistrationCode;
                u.VendorEmail = v.VendorEmail;
                u.DofascoEmail = v.DofascoEmail;
                u.nsKnox = v.nsKnox;
                u.EmailSent = v.EmailSent;
                //u.CreatedDate = v.CreatedDate;
                u.UpdatedDate = DateTime.Now;
                u.LinkGuid = v.LinkGuid;
                u.Status = v.Status;
                u.Status = "Validation Pending";
                u.PurchaseManagerApproved = false;
                u.WorldCheckApproved = false;
                u.TreasuryValidated = false;
                db.SaveChanges();
            }
        }
        public void VendorMasterCustomer_InsertOrUpdate(VendorMaster v)
        {
            DataHelper ds = new DataHelper();
            var para = new SqlParameter[]
              {
                    new SqlParameter("@Id",v.Id),
                   new SqlParameter("@BusinessName",v.BusinessName ),
                    new SqlParameter("@RegistrationCode",v.RegistrationCode),
                     new SqlParameter("@VendorEmail",v.VendorEmail),
                      new SqlParameter("@DofascoEmail",v.DofascoEmail),
                       new SqlParameter("@nsKnox",v.nsKnox),
                     new SqlParameter("@EmailSent",v.EmailSent),
                      new SqlParameter("@CreatedDate",v.CreatedDate),
                       new SqlParameter("@LinkExpired",v.LinkExpired),
              };

            ds.ExecuteStoredProcedure("proc_InsertVendorMaster", para, System.Data.CommandType.StoredProcedure, ConnectionType.Customer);
        }
        public void DeleteVendorFromCustomer(int VendorId)
        {

            DataHelper ds = new DataHelper();
            var para = new SqlParameter[]
              {
                    new SqlParameter("@VendorId",VendorId),
              };

            ds.ExecuteStoredProcedure("proc_DeleteVendorData", para, System.Data.CommandType.StoredProcedure, ConnectionType.Customer);

        }
        public int UpdatePurchaseApproval(string RegitrationCode, bool IsApprove)
        {
            var objVendorMst = db.VendorMaster.Where(p => p.RegistrationCode == RegitrationCode).FirstOrDefault();
            objVendorMst.PurchaseManagerApproved = IsApprove;
            db.SaveChanges();
            return objVendorMst.Id;
        }
        public int UpdateWorldCheckApproval(string RegitrationCode, bool IsApprove)
        {
            var objVendorMst = db.VendorMaster.Where(p => p.RegistrationCode == RegitrationCode).FirstOrDefault();
            objVendorMst.WorldCheckApproved = IsApprove;
            db.SaveChanges();
            return objVendorMst.Id;
        }
        public int UpdateStatus(string RegitrationCode, string Status)
        {
            var objVendorMst = db.VendorMaster.Where(p => p.RegistrationCode == RegitrationCode).FirstOrDefault();
            objVendorMst.Status = Status;
            db.SaveChanges();
            return objVendorMst.Id;
        }
        public int UpdateTreasuryvlidated(string RegitrationCode, bool IsApprove)
        {
            var objVendorMst = db.VendorMaster.Where(p => p.RegistrationCode == RegitrationCode).FirstOrDefault();
            objVendorMst.TreasuryValidated = IsApprove;
            db.SaveChanges();
            return objVendorMst.Id;
        }
        public int AssignedBackToRequestor(string RegitrationCode)
        {
            var objVendorMst = db.VendorMaster.Where(p => p.RegistrationCode == RegitrationCode).FirstOrDefault();
            objVendorMst.PurchaseManagerApproved = false;
            objVendorMst.WorldCheckApproved = false;
            db.SaveChanges();
            return objVendorMst.Id;
        }
        public List<VendorGridData> GetVendorGridData()
        {
            DataHelper ds = new DataHelper();
            var lst = ds.ExecuteStoredProcedure<VendorGridData>("proc_GetVendorGridData", null);
            return lst;
        }

        public List<Country> GetAllCountry()
        {
            var lst = from c in db.Country
                      select new Country
                      {
                          Id = c.Id,
                          Name = c.Name,
                      };
            var res = lst.ToList();
            res.Insert(0, new Country { Id = 0, Name = "" });
            return res;
        }
        public void SetLinkExpired(int Id)
        {

            var u = db.VendorMaster.Where(p => p.Id == Id).Single();
            u.LinkExpired = true;
            u.LinkGuid = string.Empty;
            db.SaveChanges();

        }
        #region VendorBasicInfo
        public IQueryable<VendorBasicInfo> VendorBasicInfos
        {
            get
            {
                return from v in db.VendorBasicInfo
                       select new VendorBasicInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           VendorName = v.VendorName,
                           AlternateName = v.AlternateName,
                           SCAC = v.SCAC,
                           BusinessAddress = v.BusinessAddress,
                           BusinessCity = v.BusinessCity,
                           BusinessState = v.BusinessState,
                           BusinessZip = v.BusinessZip,
                           Country = v.Country,
                           Phonenumber = v.Phonenumber,
                           Website = v.Website,
                           PaymentCurrency = v.PaymentCurrency,
                           GSTApplicable = v.GSTApplicable,
                           GSTNumber = v.GSTNumber,
                           DiversityBusinessCertification = v.DiversityBusinessCertification,
                           IsCouncilMember = v.IsCouncilMember,
                           CouncilName = v.CouncilName,
                           CreatedBy = v.CreatedBy,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }


        public void VendorBasicInfo_InsertOrUpdate(VendorBasicInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorBasicInfo
                {
                    VendorId = v.VendorId,
                    VendorName = v.VendorName,
                    AlternateName = v.AlternateName,
                    SCAC = v.SCAC,
                    BusinessAddress = v.BusinessAddress,
                    BusinessCity = v.BusinessCity,
                    BusinessState = v.BusinessState,
                    BusinessZip = v.BusinessZip,
                    Country = v.Country,
                    Phonenumber = v.Phonenumber,
                    Website = v.Website,
                    PaymentCurrency = v.PaymentCurrency,
                    GSTApplicable = v.GSTApplicable,
                    GSTNumber = v.GSTNumber,
                    DiversityBusinessCertification = v.DiversityBusinessCertification,
                    IsCouncilMember = v.IsCouncilMember,
                    CouncilName = v.CouncilName,
                    CreatedBy = "",
                    CreatedDate = DateTime.Now,
                };

                db.VendorBasicInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorBasicInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.VendorName = v.VendorName;
                u.AlternateName = v.AlternateName;
                u.SCAC = v.SCAC;
                u.BusinessAddress = v.BusinessAddress;
                u.BusinessCity = v.BusinessCity;
                u.BusinessState = v.BusinessState;
                u.BusinessZip = v.BusinessZip;
                u.Country = v.Country;
                u.Phonenumber = v.Phonenumber;
                u.Website = v.Website;
                u.PaymentCurrency = v.PaymentCurrency;
                u.GSTApplicable = v.GSTApplicable;
                u.GSTNumber = v.GSTNumber;
                u.DiversityBusinessCertification = v.DiversityBusinessCertification;
                u.IsCouncilMember = v.IsCouncilMember;
                u.CouncilName = v.CouncilName;
                db.SaveChanges();
            }
        }
        public void VendorBasicInfo_TransferData(VendorBasicInfo v)
        {
            var u = db.VendorBasicInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u == null)
            {
                var i = new EF.VendorBasicInfo
                {
                    VendorId = v.VendorId,
                    VendorName = v.VendorName,
                    AlternateName = v.AlternateName,
                    SCAC = v.SCAC,
                    BusinessAddress = v.BusinessAddress,
                    BusinessCity = v.BusinessCity,
                    BusinessState = v.BusinessState,
                    BusinessZip = v.BusinessZip,
                    Country = v.Country,
                    Phonenumber = v.Phonenumber,
                    Website = v.Website,
                    PaymentCurrency = v.PaymentCurrency,
                    GSTApplicable = v.GSTApplicable,
                    GSTNumber = v.GSTNumber,
                    DiversityBusinessCertification = v.DiversityBusinessCertification,
                    IsCouncilMember = v.IsCouncilMember,
                    CouncilName = v.CouncilName,
                    CreatedBy = "",
                    CreatedDate = DateTime.Now,
                };

                db.VendorBasicInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.VendorName = v.VendorName;
                u.AlternateName = v.AlternateName;
                u.SCAC = v.SCAC;
                u.BusinessAddress = v.BusinessAddress;
                u.BusinessCity = v.BusinessCity;
                u.BusinessState = v.BusinessState;
                u.BusinessZip = v.BusinessZip;
                u.Country = v.Country;
                u.Phonenumber = v.Phonenumber;
                u.Website = v.Website;
                u.PaymentCurrency = v.PaymentCurrency;
                u.GSTApplicable = v.GSTApplicable;
                u.GSTNumber = v.GSTNumber;
                u.DiversityBusinessCertification = v.DiversityBusinessCertification;
                u.IsCouncilMember = v.IsCouncilMember;
                u.CouncilName = v.CouncilName;
                db.SaveChanges();
            }
        }
        #endregion

        #region VendorFinancialInfo
        public IQueryable<VendorFinancialInfo> VendorFinancialInfos
        {
            get
            {
                return from v in db.VendorFinancialInfo
                       select new VendorFinancialInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           ContactName = v.ContactName,
                           ContactEmail = v.ContactEmail,
                           ContactPhone = v.ContactPhone,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorFinancialInfo_InsertOrUpdate(VendorFinancialInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorFinancialInfo
                {
                    VendorId = v.VendorId,
                    ContactName = v.ContactName,
                    ContactEmail = v.ContactEmail,
                    ContactPhone = v.ContactPhone,
                    CreatedDate = DateTime.Now,
                };

                db.VendorFinancialInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorFinancialInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.ContactName = v.ContactName;
                u.ContactEmail = v.ContactEmail;
                u.ContactPhone = v.ContactPhone;
                db.SaveChanges();
            }
        }
        public void VendorFinancialInfo_TransferData(VendorFinancialInfo v)
        {
            var u = db.VendorFinancialInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorFinancialInfo
                {
                    VendorId = v.VendorId,
                    ContactName = v.ContactName,
                    ContactEmail = v.ContactEmail,
                    ContactPhone = v.ContactPhone,
                    CreatedDate = DateTime.Now,
                };

                db.VendorFinancialInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                
                u.VendorId = v.VendorId;
                u.ContactName = v.ContactName;
                u.ContactEmail = v.ContactEmail;
                u.ContactPhone = v.ContactPhone;
                db.SaveChanges();
            }
        }
        #endregion

        #region VendorPRimarySalesInfo

        public IQueryable<VendorPrimarySalesInfo> VendorPrimarySalesInfos
        {
            get
            {
                return from v in db.VendorPrimarySalesInfo
                       select new VendorPrimarySalesInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           ContactName = v.ContactName,
                           ContactEmail = v.ContactEmail,
                           ContactPhone = v.ContactPhone,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorPrimarySalesInfo_InsertOrUpdate(VendorPrimarySalesInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorPrimarySalesInfo
                {
                    VendorId = v.VendorId,
                    ContactName = v.ContactName,
                    ContactEmail = v.ContactEmail,
                    ContactPhone = v.ContactPhone,
                    CreatedDate = DateTime.Now,
                };

                db.VendorPrimarySalesInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorPrimarySalesInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.ContactName = v.ContactName;
                u.ContactEmail = v.ContactEmail;
                u.ContactPhone = v.ContactPhone;
                db.SaveChanges();
            }
        }
        public void VendorPrimarySalesInfo_TransferData(VendorPrimarySalesInfo v)
        {
            var u = db.VendorPrimarySalesInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorPrimarySalesInfo
                {
                    VendorId = v.VendorId,
                    ContactName = v.ContactName,
                    ContactEmail = v.ContactEmail,
                    ContactPhone = v.ContactPhone,
                    CreatedDate = DateTime.Now,
                };

                db.VendorPrimarySalesInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.ContactName = v.ContactName;
                u.ContactEmail = v.ContactEmail;
                u.ContactPhone = v.ContactPhone;
                db.SaveChanges();
            }
        }
        #endregion

        #region VendorSubmittedbyInfo
        public IQueryable<VendorSubmittedByInfo> VendorSubmittedByInfos
        {
            get
            {
                return from v in db.VendorSubmittedByInfo
                       select new VendorSubmittedByInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           SubmittedByName = v.SubmittedByName,
                           Email = v.Email,
                           DofascoEmail = v.DofascoEmail,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorSubmittedByInfo_InsertOrUpdate(VendorSubmittedByInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorSubmittedByInfo
                {
                    VendorId = v.VendorId,
                    SubmittedByName = v.SubmittedByName,
                    Email = v.Email,
                    DofascoEmail = v.DofascoEmail,
                    CreatedDate = DateTime.Now,
                };

                db.VendorSubmittedByInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorSubmittedByInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.SubmittedByName = v.SubmittedByName;
                u.Email = v.Email;
                u.DofascoEmail = v.DofascoEmail;
                db.SaveChanges();
            }
        }
        public void VendorSubmittedByInfo_TransferData(VendorSubmittedByInfo v)
        {
            var u = db.VendorSubmittedByInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorSubmittedByInfo
                {
                    VendorId = v.VendorId,
                    SubmittedByName = v.SubmittedByName,
                    Email = v.Email,
                    DofascoEmail = v.DofascoEmail,
                    CreatedDate = DateTime.Now,
                };

                db.VendorSubmittedByInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.SubmittedByName = v.SubmittedByName;
                u.Email = v.Email;
                u.DofascoEmail = v.DofascoEmail;
                db.SaveChanges();
            }
        }
        #endregion

        #region VendorRemittanceInfo
        public IQueryable<VendorRemittanceInfo> VendorRemittanceInfos
        {
            get
            {
                return from v in db.VendorRemittanceInfo
                       select new VendorRemittanceInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           MailingAddress = v.MailingAddress,
                           MailingCity = v.MailingCity,
                           State = v.State,
                           Zip = v.Zip,
                           Country = v.Country,
                           Email = v.Email,
                           SpecialPayment = v.SpecialPayment,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorRemittanceInfo_InsertOrUpdate(VendorRemittanceInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorRemittanceInfo
                {
                    VendorId = v.VendorId,
                    MailingAddress = v.MailingAddress,
                    MailingCity = v.MailingCity,
                    State = v.State,
                    Zip = v.Zip,
                    Country = v.Country,
                    Email = v.Email,
                    SpecialPayment = v.SpecialPayment,
                    CreatedDate = DateTime.Now,
                };

                db.VendorRemittanceInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorRemittanceInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.MailingAddress = v.MailingAddress;
                u.MailingCity = v.MailingCity;
                u.State = v.State;
                u.Zip = v.Zip;
                u.Country = v.Country;
                u.Email = v.Email;
                u.SpecialPayment = v.SpecialPayment;

                db.SaveChanges();
            }
        }
        public void VendorRemittanceInfo_TransferData(VendorRemittanceInfo v)
        {
            var u = db.VendorRemittanceInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorRemittanceInfo
                {
                    VendorId = v.VendorId,
                    MailingAddress = v.MailingAddress,
                    MailingCity = v.MailingCity,
                    State = v.State,
                    Zip = v.Zip,
                    Country = v.Country,
                    Email = v.Email,
                    SpecialPayment = v.SpecialPayment,
                    CreatedDate = DateTime.Now,
                };

                db.VendorRemittanceInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.MailingAddress = v.MailingAddress;
                u.MailingCity = v.MailingCity;
                u.State = v.State;
                u.Zip = v.Zip;
                u.Country = v.Country;
                u.Email = v.Email;
                u.SpecialPayment = v.SpecialPayment;

                db.SaveChanges();
            }
        }
        #endregion

        #region VendorBankingInfo
        public IQueryable<VendorBankingInfo> VendorBankingInfos
        {
            get
            {
                return from v in db.VendorBankingInfo
                       select new VendorBankingInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           Name = v.Name,
                           Address = v.Address,
                           City = v.City,
                           State = v.State,
                           Zip = v.Zip,
                           Country = v.Country,
                           InstitutionNo = v.InstitutionNo,
                           TransitNo = v.TransitNo,
                           BankAccountNo = v.BankAccountNo,
                           ABA_RoutingNumber = v.ABA_RoutingNumber,
                           Swift_BICCode = v.Swift_BICCode,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorBankingInfo_InsertOrUpdate(VendorBankingInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorBankingInfo
                {
                    VendorId = v.VendorId,
                    Name = v.Name,
                    Address = v.Address,
                    City = v.City,
                    State = v.State,
                    Zip = v.Zip,
                    Country = v.Country,
                    InstitutionNo = v.InstitutionNo,
                    TransitNo = v.TransitNo,
                    BankAccountNo = v.BankAccountNo,
                    ABA_RoutingNumber = v.ABA_RoutingNumber,
                    Swift_BICCode = v.Swift_BICCode,
                    CreatedDate = DateTime.Now,
                };

                db.VendorBankingInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorBankingInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.Name = v.Name;
                u.Address = v.Address;
                u.City = v.City;
                u.State = v.State;
                u.Zip = v.Zip;
                u.Country = v.Country;
                u.InstitutionNo = v.InstitutionNo;
                u.TransitNo = v.TransitNo;
                u.BankAccountNo = v.BankAccountNo;
                u.ABA_RoutingNumber = v.ABA_RoutingNumber;
                u.Swift_BICCode = v.Swift_BICCode;

                db.SaveChanges();
            }
        }
        public void VendorBankingInfo_TransferData(VendorBankingInfo v)
        {
            var u = db.VendorBankingInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorBankingInfo
                {
                    VendorId = v.VendorId,
                    Name = v.Name,
                    Address = v.Address,
                    City = v.City,
                    State = v.State,
                    Zip = v.Zip,
                    Country = v.Country,
                    InstitutionNo = v.InstitutionNo,
                    TransitNo = v.TransitNo,
                    BankAccountNo = v.BankAccountNo,
                    ABA_RoutingNumber = v.ABA_RoutingNumber,
                    Swift_BICCode = v.Swift_BICCode,
                    CreatedDate = DateTime.Now,
                };

                db.VendorBankingInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.Name = v.Name;
                u.Address = v.Address;
                u.City = v.City;
                u.State = v.State;
                u.Zip = v.Zip;
                u.Country = v.Country;
                u.InstitutionNo = v.InstitutionNo;
                u.TransitNo = v.TransitNo;
                u.BankAccountNo = v.BankAccountNo;
                u.ABA_RoutingNumber = v.ABA_RoutingNumber;
                u.Swift_BICCode = v.Swift_BICCode;

                db.SaveChanges();
            }
        }
        #endregion

        #region VendorAttachmentInfo
        public IQueryable<VendorAttachmentInfo> VendorAttachmentInfos
        {
            get
            {
                return from v in db.VendorAttachmentInfo
                       select new VendorAttachmentInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           FileName = v.FileName,
                           FileType = v.FileType,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorAttachmentInfo_InsertOrUpdate(VendorAttachmentInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorAttachmentInfo
                {
                    VendorId = v.VendorId,
                    FileName = v.FileName,
                    FileType = v.FileType,
                    CreatedDate = DateTime.Now,
                };

                db.VendorAttachmentInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorAttachmentInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.FileName = v.FileName;
                u.FileType = v.FileType;

                db.SaveChanges();
            }
        }
        public void VendorAttachmentInfo_Remove(VendorAttachmentInfo v)
        {
            var e = db.VendorAttachmentInfo.Where(p => p.Id == v.Id).FirstOrDefault();
            if (e != null)
            {
                db.VendorAttachmentInfo.DeleteObject(e);
                db.SaveChanges();
            }
        }
        public void VendorAttachmentInfo_TransferData(int VendorId)
        {
            var e = db.VendorAttachmentInfo.Where(p => p.VendorId == VendorId).ToList();
            if (e != null)
            {
                foreach (var a in e)
                {
                    db.VendorAttachmentInfo.DeleteObject(a);
                    db.SaveChanges();
                }
            }
        }
        #endregion

        #region VendorPurchasingInfo
        public IQueryable<VendorPurchasingInfo> VendorPurchasingInfos
        {
            get
            {
                return from v in db.VendorPurchasingInfo
                       select new VendorPurchasingInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           TradingPartner = v.TradingPartner,
                           AccountGroupName = v.AccountGroupName,
                           TypeofVendorRequest = v.TypeofVendorRequest,
                           ConflictOfInterest = v.ConflictOfInterest,
                           PaymentTerms = v.PaymentTerms,
                           SpendTreelevel1 = v.SpendTreelevel1,
                           SpendTreeLevel2 = v.SpendTreeLevel2,
                           SpendTreeLevel3 = v.SpendTreeLevel3,
                           SpendTreeLevel4 = v.SpendTreeLevel4,
                           SAPBusType = v.SAPBusType,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorPurchasingInfo_InsertOrUpdate(VendorPurchasingInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorPurchasingInfo
                {
                    VendorId = v.VendorId,
                    TradingPartner = v.TradingPartner,
                    AccountGroupName = v.AccountGroupName,
                    TypeofVendorRequest = v.TypeofVendorRequest,
                    ConflictOfInterest = v.ConflictOfInterest,
                    PaymentTerms = v.PaymentTerms,
                    SpendTreelevel1 = v.SpendTreelevel1,
                    SpendTreeLevel2 = v.SpendTreeLevel2,
                    SpendTreeLevel3 = v.SpendTreeLevel3,
                    SpendTreeLevel4 = v.SpendTreeLevel4,
                    SAPBusType = v.SAPBusType,
                    CreatedDate = DateTime.Now,
                };

                db.VendorPurchasingInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorPurchasingInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.TradingPartner = v.TradingPartner;
                u.AccountGroupName = v.AccountGroupName;
                u.TypeofVendorRequest = v.TypeofVendorRequest;
                u.ConflictOfInterest = v.ConflictOfInterest;
                u.PaymentTerms = v.PaymentTerms;
                u.SpendTreelevel1 = v.SpendTreelevel1;
                u.SpendTreeLevel2 = v.SpendTreeLevel2;
                u.SpendTreeLevel3 = v.SpendTreeLevel3;
                u.SpendTreeLevel4 = v.SpendTreeLevel4;
                u.SAPBusType = v.SAPBusType;
                db.SaveChanges();
            }
        }
        public void VendorPurchasingInfo_TransferData(VendorPurchasingInfo v)
        {
            var u = db.VendorPurchasingInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorPurchasingInfo
                {
                    VendorId = v.VendorId,
                    TradingPartner = v.TradingPartner,
                    AccountGroupName = v.AccountGroupName,
                    TypeofVendorRequest = v.TypeofVendorRequest,
                    ConflictOfInterest = v.ConflictOfInterest,
                    PaymentTerms = v.PaymentTerms,
                    SpendTreelevel1 = v.SpendTreelevel1,
                    SpendTreeLevel2 = v.SpendTreeLevel2,
                    SpendTreeLevel3 = v.SpendTreeLevel3,
                    SpendTreeLevel4 = v.SpendTreeLevel4,
                    SAPBusType = v.SAPBusType,
                    CreatedDate = DateTime.Now,
                };

                db.VendorPurchasingInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                u.VendorId = v.VendorId;
                u.TradingPartner = v.TradingPartner;
                u.AccountGroupName = v.AccountGroupName;
                u.TypeofVendorRequest = v.TypeofVendorRequest;
                u.ConflictOfInterest = v.ConflictOfInterest;
                u.PaymentTerms = v.PaymentTerms;
                u.SpendTreelevel1 = v.SpendTreelevel1;
                u.SpendTreeLevel2 = v.SpendTreeLevel2;
                u.SpendTreeLevel3 = v.SpendTreeLevel3;
                u.SpendTreeLevel4 = v.SpendTreeLevel4;
                u.SAPBusType = v.SAPBusType;
                db.SaveChanges();
            }
        }
        #endregion
        #region VendorWorkflowInfo
        public IQueryable<VendorWorkFlowInfo> VendorWorkFlowInfos
        {
            get
            {
                return from v in db.VendorWorkFlowInfo
                       select new VendorWorkFlowInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           RequestorName = v.RequestorName,
                           PurchasingManager = v.PurchasingManager,
                           VendorNumber = v.VendorNumber,
                           PurchaseComments = v.PurchaseComments,
                           CreatedDate = v.CreatedDate,
                           WorldCheckApprover = v.WorldCheckApprover
                       };
            }
        }

        public void VendorWorkFlowInfo_InsertOrUpdate(VendorWorkFlowInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorWorkFlowInfo
                {
                    VendorId = v.VendorId,
                    RequestorName = v.RequestorName,
                    PurchasingManager = v.PurchasingManager,
                    VendorNumber = v.VendorNumber,
                    PurchaseComments = v.PurchaseComments,
                    CreatedDate = DateTime.Now,
                    WorldCheckApprover = v.WorldCheckApprover
                };

                db.VendorWorkFlowInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
                var u = db.VendorWorkFlowInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.RequestorName = v.RequestorName;
                u.PurchasingManager = v.PurchasingManager;
                u.VendorNumber = v.VendorNumber;
                u.PurchaseComments = v.PurchaseComments;
                u.WorldCheckApprover = v.WorldCheckApprover;
                db.SaveChanges();
            }
        }
        public void VendorWorkFlowInfo_TransferData(VendorWorkFlowInfo v)
        {
            var u = db.VendorWorkFlowInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorWorkFlowInfo
                {
                    VendorId = v.VendorId,
                    RequestorName = v.RequestorName,
                    PurchasingManager = v.PurchasingManager,
                    VendorNumber = v.VendorNumber,
                    PurchaseComments = v.PurchaseComments,
                    CreatedDate = DateTime.Now,
                    WorldCheckApprover = v.WorldCheckApprover
                };

                db.VendorWorkFlowInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }


            else
            {
               
                u.VendorId = v.VendorId;
                u.RequestorName = v.RequestorName;
                u.PurchasingManager = v.PurchasingManager;
                u.VendorNumber = v.VendorNumber;
                u.PurchaseComments = v.PurchaseComments;
                u.WorldCheckApprover = v.WorldCheckApprover;
                db.SaveChanges();
            }
        }
        #endregion

        #region VendorTreasuryInformation

        public IQueryable<VendorTreasuryInfo> VendorTreasuryInfos
        {
            get
            {
                return from v in db.VendorTreasuryInfo
                       select new VendorTreasuryInfo
                       {
                           Id = v.Id,
                           VendorId = v.VendorId,
                           ActionerName = v.ActionerName,
                           VendorNumber = v.VendorNumber,
                           Validated = v.Validated,
                           ChecklistInfo1 = v.ChecklistInfo1,
                           ChecklistInfo2 = v.ChecklistInfo2,
                           Level2ApproverName = v.Level2ApproverName,
                           Level2ChecklistInfo = v.Level2ChecklistInfo,
                           Level2Comments = v.Level2Comments,
                           CreatedDate = v.CreatedDate,
                       };
            }
        }

        public void VendorTreasuryInfo_InsertOrUpdate(VendorTreasuryInfo v)
        {
            if (v.Id == 0)
            {
                var i = new EF.VendorTreasuryInfo
                {
                    VendorId = v.VendorId,
                    ActionerName = v.ActionerName,
                    VendorNumber = v.VendorNumber,
                    Validated = v.Validated,
                    ChecklistInfo1 = v.ChecklistInfo1,
                    ChecklistInfo2 = v.ChecklistInfo2,
                    Level2ApproverName = v.Level2ApproverName,
                    Level2ChecklistInfo = v.Level2ChecklistInfo,
                    Level2Comments = v.Level2Comments,
                    CreatedDate = DateTime.Now,
                };
                db.VendorTreasuryInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }
            else
            {
                var u = db.VendorTreasuryInfo.Where(p => p.Id == v.Id).Single();
                u.VendorId = v.VendorId;
                u.ActionerName = v.ActionerName;
                u.VendorNumber = v.VendorNumber;
                u.Validated = v.Validated;
                u.ChecklistInfo1 = v.ChecklistInfo1;
                u.ChecklistInfo2 = v.ChecklistInfo2;
                u.Level2ApproverName = v.Level2ApproverName;
                u.Level2ChecklistInfo = v.Level2ChecklistInfo;
                u.Level2Comments = v.Level2Comments;
                db.SaveChanges();
            }
        }
        public void VendorTreasuryInfo_TransferData(VendorTreasuryInfo v)
        {
            var u = db.VendorTreasuryInfo.Where(p => p.VendorId == v.VendorId).FirstOrDefault();
            if (u==null)
            {
                var i = new EF.VendorTreasuryInfo
                {
                    VendorId = v.VendorId,
                    ActionerName = v.ActionerName,
                    VendorNumber = v.VendorNumber,
                    Validated = v.Validated,
                    ChecklistInfo1 = v.ChecklistInfo1,
                    ChecklistInfo2 = v.ChecklistInfo2,
                    Level2ApproverName = v.Level2ApproverName,
                    Level2ChecklistInfo = v.Level2ChecklistInfo,
                    Level2Comments = v.Level2Comments,
                    CreatedDate = DateTime.Now,
                };
                db.VendorTreasuryInfo.AddObject(i);
                db.SaveChanges();
                v.Id = i.Id;
            }
            else
            {
                u.VendorId = v.VendorId;
                u.ActionerName = v.ActionerName;
                u.VendorNumber = v.VendorNumber;
                u.Validated = v.Validated;
                u.ChecklistInfo1 = v.ChecklistInfo1;
                u.ChecklistInfo2 = v.ChecklistInfo2;
                u.Level2ApproverName = v.Level2ApproverName;
                u.Level2ChecklistInfo = v.Level2ChecklistInfo;
                u.Level2Comments = v.Level2Comments;
                db.SaveChanges();
            }
        }
        #endregion
    }
}
