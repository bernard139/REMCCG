using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class RemittanceDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public string RemittanceType { get; set; } // Enum for different types of offerings
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
    }

    public class RemittanceModel
    {
        public int ID { get; set; }
        public string RemittanceType { get; set; } // Enum for different types of offerings
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
    }
}
