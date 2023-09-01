using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.Models
{
    public class MailSettingsOld
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string FromEmail { get; set; }
    }
    public class MailSettings
    {
        public MailSettings()
        {
            CopyAddresses = new HashSet<string>();
        }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public SmtpCredentials Credentials { get; set; }
        public ICollection<string> CopyAddresses { get; set; }
        public string SubjectTemplate { get; set; }
    }
}
