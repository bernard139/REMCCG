using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Remittance
    {
        public int ID { get; set; }
        public RemittanceType Type { get; set; } // Enum for different types of offerings
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
    }
}
