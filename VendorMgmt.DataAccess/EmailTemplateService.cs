using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorMgmt.DataAccess.Model;
using EF = VendorMgmt.Model;
namespace VendorMgmt.DataAccess
{
    public class EmailTemplateService : ConnectionHelper
    {
        EF.VendorEntities db = null;
        public EmailTemplateService()
        {
            db = new EF.VendorEntities(EntityConnectionString);
        }
        public EmailTemplateService(ObjectContext context)
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
        public IQueryable<EmailTemplate> EmailTemplates
        {
            get
            {
                return from e in db.EmailTemplate
                       select new EmailTemplate
                       {
                           Id = e.Id,
                           Name = e.Name,
                           SenderName = e.SenderName,
                           SenderEmail = e.SenderEmail,
                           EmailSubject = e.EmailSubject,
                           EmailBody = e.EmailBody,
                           CreatedDate = e.CreatedDate,
                       };
            }
        }
        public EmailTemplate EmailTemplateById(int Id)
        {

            var template = from e in db.EmailTemplate
                           where e.Id == Id
                           select new EmailTemplate
                           {
                               Id = e.Id,
                               Name = e.Name,
                               SenderName = e.SenderName,
                               SenderEmail = e.SenderEmail,
                               EmailSubject = e.EmailSubject,
                               EmailBody = e.EmailBody,
                               CreatedDate = e.CreatedDate,
                           };
            return template.FirstOrDefault();
        }
        public EmailTemplate EmailTemplateByName(string Name)
        {

            var template = from e in db.EmailTemplate
                           where e.Name == Name
                           select new EmailTemplate
                           {
                               Id = e.Id,
                               Name = e.Name,
                               SenderName = e.SenderName,
                               SenderEmail = e.SenderEmail,
                               EmailSubject = e.EmailSubject,
                               EmailBody = e.EmailBody,
                               CreatedDate = e.CreatedDate,
                           };
            return template.FirstOrDefault();
        }
        public void EmailTemplate_InsertOrUpdate(EmailTemplate e)
        {
            if (e.Id == 0)
            {
                var i = new EF.EmailTemplate
                {
                    Name = e.Name,
                    SenderName = e.SenderName,
                    SenderEmail = e.SenderEmail,
                    EmailSubject = e.EmailSubject,
                    EmailBody = e.EmailBody,
                    CreatedDate = e.CreatedDate,
                };

                db.EmailTemplate.AddObject(i);
                db.SaveChanges();
                e.Id = i.Id;
            }


            else
            {
                var u = db.EmailTemplate.Where(p => p.Id == e.Id).Single();
                u.Name = e.Name;
                u.SenderName = e.SenderName;
                u.SenderEmail = e.SenderEmail;
                u.EmailSubject = e.EmailSubject;
                u.EmailBody = e.EmailBody;
                db.SaveChanges();
            }
        }
        public void DeleteEmailTemplatebyId(int Id)
        {
            var e = db.EmailTemplate.Where(p => p.Id == Id).Single();
            db.EmailTemplate.DeleteObject(e);
            db.SaveChanges();
        }

    }
}
