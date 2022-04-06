using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorMgmt.DataAccess.Model
{
    public class EmailTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class EmailTemplateViewModel
    {
        public List<EmailTemplate> lstResults { get; set; }
    }
}
